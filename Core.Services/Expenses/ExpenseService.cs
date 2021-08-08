using Core.Services.Expenses.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Expenses
{
    public class ExpenseService : IExpenseService
    {
        public ExpenseService()
        {

        }

        public List<string> GetAllExpenses()
        {
            List<string> expenses = new List<string> { "shopping", "Watch Movie", "Gardening" };
            return expenses;
        }
    }
}
