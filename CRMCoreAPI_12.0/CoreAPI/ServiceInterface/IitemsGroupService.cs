using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    public interface IitemsGroupService
    {
        string GetItems(long companyId, long loggedInUserId, string sortBy, string sortExp, int pageSize, int pageNumber);

        //string SaveItem(string ItemData);
    }
}
