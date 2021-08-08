using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Expenses.Interfaces
{
    public interface IExpenseService
    {
        List<string> GetAllExpenses();
    }
}
