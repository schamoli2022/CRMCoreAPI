using CRMCoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CRMCoreAPI.Services.ExpenseServices;

namespace CRMCoreAPI.Services
{
    public interface IExpenseServices
    {
        ExpenseItems AddExpenseItems(ExpenseItems items);

        Dictionary<string, ExpenseItems> GetExpenseItems();

        CRMDynamicData GetExpenseList(int companyId);

        void ValidateData(string leadData);
    }
}
