﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeCamperVansModel.AccessModels
{
    [Serializable]
    public class GetAllLocationForClientIDForMobileRequest
    {
        public int ClientID { get; set; }
    }
}
