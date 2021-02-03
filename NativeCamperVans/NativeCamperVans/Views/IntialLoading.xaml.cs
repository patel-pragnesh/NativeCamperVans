using NativeCamperVans.Popups;
using NativeCamperVansController;
using NativeCamperVansModel;
using NativeCamperVansModel.AccessModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NativeCamperVans.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntialLoading : ContentPage
    {
        private ApiToken apiToken;
        private GetClientDetailsForMobileResponse getClientDetailsForMobile;
        public IntialLoading()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(300);
            GetClientSecretTokenRequest getClientSecretTokenRequest = new GetClientSecretTokenRequest();
            getClientSecretTokenRequest.ClientId = Constants.ClientId;
            ApiController apiController = new ApiController();
            GetClientSecretTokenResponse clientSecretTokenResponse = null;
            try
            {
                clientSecretTokenResponse = apiController.GetClientSecretToken(getClientSecretTokenRequest);
            }
            catch (Exception ex)
            {
                await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
            }
            if (clientSecretTokenResponse != null)
            {
                GetAccessTokenRequest tokenRequest = new GetAccessTokenRequest();
                tokenRequest.client_id = clientSecretTokenResponse.apiConsumerId;
                tokenRequest.client_secret = clientSecretTokenResponse.apiConsumerSecret;
                tokenRequest.grant_type = "client_credentials";

                try
                {
                    apiToken = apiController.GetAccessToken(tokenRequest);
                }
                catch (Exception ex)
                {
                    await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                }
                if (apiToken != null)
                {
                    string _token = apiToken.access_token;
                    CommonController commonController = new CommonController();

                    try
                    {
                        getClientDetailsForMobile = commonController.GetClientDetailsForMobile(_token);
                    }
                    catch (Exception ex)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
                    }


                    if (getClientDetailsForMobile != null)
                    {
                        if (getClientDetailsForMobile.admin != null)
                        {
                            Constants.admin = getClientDetailsForMobile.admin;
                        }
                    }

                    if (App.Current.Properties.ContainsKey("currentToken"))
                    {
                        App.Current.Properties["currentToken"] = _token;
                    }
                    else
                    {
                        App.Current.Properties.Add("currentToken", _token);
                    }
                }

            }
            

            if ((int)App.Current.Properties["CustomerId"] == 0)
            {
                await Navigation.PushAsync(new WelcomPage());
            }
            else
            {
                await Navigation.PushAsync(new HomePage());
            }


        }
    }
}