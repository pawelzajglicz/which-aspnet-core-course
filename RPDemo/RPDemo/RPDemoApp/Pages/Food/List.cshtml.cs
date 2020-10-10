using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RPDemoApp.Pages.Food
{
    public class ListModel : PageModel
    {
        private readonly IFoodData foodData;

        public List<FoodModel> Food { get; set; }

        public ListModel(IFoodData foodData)
        {
            this.foodData = foodData;
        }

        public async Task OnGet()
        {
            Food = await foodData.GetFood();
        }
    }
}
