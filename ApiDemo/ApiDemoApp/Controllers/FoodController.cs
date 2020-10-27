using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodData foodData;

        public FoodController(IFoodData foodData)
        {
            this.foodData = foodData;
        }

        [HttpGet]
        public async Task<List<FoodModel>> Get()
        {
            return await foodData.GetFood();
        }
    }
}
