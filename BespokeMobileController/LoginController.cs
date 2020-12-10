using BespokeMobileModel;
using BespokeMobileServices.ApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BespokeMobileController
{
    public class LoginController
    {
        LoginService loginservice;
        public LoginController()
        {
            loginservice = new LoginService();
        }

        public CutomerAuthContext CheckLogin(CustomerLogin loginCustomer, string token)
        {
            return loginservice.CheckLogin(loginCustomer,token);
        }
    }
}
