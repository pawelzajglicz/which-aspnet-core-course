using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using MVCDemoApp.Models;

namespace MVCDemoApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IFoodData foodData;
        private readonly IOrderData orderData;

        public OrdersController(IFoodData foodData, IOrderData orderData)
        {
            this.foodData = foodData;
            this.orderData = orderData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var food = await foodData.GetFood();

            OrderCreateModel model = new OrderCreateModel();

            food.ForEach(f =>
            {
                model.FoodItems.Add(new SelectListItem { Value = f.Id.ToString(), Text = f.Title });
            });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var food = await foodData.GetFood();

            order.Total = order.Quantity * food.Where(f => f.Id == order.FoodId).First().Price;

            int id = await orderData.CreateOrder(order);

            return RedirectToAction("Display", new { id });
        }

        public async Task<IActionResult> Display(int id)
        {
            OrderDisplayModel displayOrder = new OrderDisplayModel();
            displayOrder.Order = await orderData.GetOrderById(id);

            if (displayOrder.Order != null)
            {
                var food = await foodData.GetFood();

                displayOrder.ItemPurchased = food.Where(f => f.Id == displayOrder.Order.FoodId).FirstOrDefault()?.Title;
            }

            return View(displayOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string orderName)
        {
            await orderData.UpdateOrderName(id, orderName);

            return RedirectToAction("Display", new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await orderData.GetOrderById(id);

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(OrderModel order)
        {
            await orderData.DeleteOrder(order.Id);

            return RedirectToAction("Create");
        }
    }
}
