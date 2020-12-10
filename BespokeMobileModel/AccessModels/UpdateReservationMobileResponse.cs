using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BespokeMobileModel.AccessModels
{
    [Serializable]
    public class UpdateReservationMobileResponse
    {
        public string ReservationNumber { get; set; }

        public ApiMessage message { get; set; }
    }
}
