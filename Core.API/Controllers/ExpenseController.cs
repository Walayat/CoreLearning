using Core.API.Cache;
using Core.Services.Expenses.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IExpenseService _expenseService;

        public ExpenseController(IDistributedCache distributedCache, IExpenseService expenseService)
        {
            _distributedCache = distributedCache;
            _expenseService = expenseService;
        }

        List<string> expenses = new List<string> { "shopping", "Watch Movie", "Gardening" };

        [HttpGet]
        [Cached(600)]
        [Route("GetAllExpenses")]
        public async Task<IActionResult> GetAll()
        {
            List<string> myExpenses = new List<string>();
            bool IsCached = false;
            string cachedExpenseString = string.Empty;

            cachedExpenseString = await _distributedCache.GetStringAsync("_GetAllExpenses");
            if (!string.IsNullOrEmpty(cachedExpenseString))
            {
                // loaded data from the redis cache.
                myExpenses = JsonSerializer.Deserialize<List<string>>(cachedExpenseString);
                IsCached = true;
            }
            else
            {
                // loading from code (in real-time from database)
                // then saving to the redis cache 
                myExpenses = _expenseService.GetAllExpenses();
                IsCached = false;
                cachedExpenseString = JsonSerializer.Serialize<List<string>>(myExpenses);
                await _distributedCache.SetStringAsync("_GetAllExpenses", cachedExpenseString);
            }

            return Ok(new { IsCached, myExpenses });
        }
    }
}
