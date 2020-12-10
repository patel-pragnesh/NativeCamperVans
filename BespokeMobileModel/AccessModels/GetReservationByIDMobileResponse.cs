using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BespokeMobileModel.AccessModels
{
    [Serializable]
    public class GetReservationByIDMobileResponse
    {
        public ReservationViewModel reservationData { get; set; }
        public VehicleTypeWithRatesViewModel vehicleTypeModel { get; set; }
        public bool isTimerVisible { get; set; }

        public ApiMessage message { get; set; }
    }
}
