﻿using NativeCamperVansModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeCamperVans
{
    public class Constants
    {
        //public static int ClientId = 971;
        //public static int ClientId = 513;
        public static int ClientId = 369;

        //public static int ClientId = 1028;
        //public static int ClientId = 1122;
        //public static int ClientId = 975;
        //public static int ClientId = 486;

        public static Admin admin = null;
        public static CutomerAuthContext cutomerAuthContext = null;
        public static CustomerReview customerDetails = null;
        public static List<int> countriesHasState = new List<int>() { 144, 121, 33, 34, 103, 198, 202, 69, 212, 2 };


        // using for find home page or not to enable back key
        public static bool IsHome = false;
        public static bool IsRegisteredandNotLogin = false;
        public void setAdmin(Admin admi)
        {
            admin = admi;
        }
    }
}
