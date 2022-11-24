using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceInterface
{
    public interface IBrandService
    {
        /// <summary>
        /// Get All Brand listing
        /// </summary>
        /// <returns></returns>
        string GetBrands(long CompanyID, long userid, string sortBy, string sortExp, int pageSize, int pageNum, long brandid = 0, string brandname = "");

        /// <summary>
        /// Implementation of save lead
        /// </summary>
        /// <param name="brandData"></param>
        /// <returns></returns>
        string SaveBrand(string brandData, long CompanyId, long UserId);
    }
}
