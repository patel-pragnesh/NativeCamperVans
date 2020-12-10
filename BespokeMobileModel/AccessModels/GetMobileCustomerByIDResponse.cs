using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BespokeMobileModel.AccessModels
{
    [Serializable]
    public class GetMobileCustomerByIDResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomerReview customerDetails { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ApiMessage message { get; set; }
    }
}
