﻿using NativeCamperVans.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using NativeCamperVansModel.AccessModels;
using NativeCamperVansController;
using Rg.Plugins.Popup.Services;
using NativeCamperVansModel;
using NativeCamperVans.Popups;

namespace NativeCamperVans
{
    public partial class App : Xamarin.Forms.Application
    {
        private ApiToken apiToken;
        private GetClientDetailsForMobileResponse getClientDetailsForMobile;

        public App(string pagename, string data)
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            InitializeComponent();

            if (!App.Current.Properties.ContainsKey("CustomerId"))
            {
                App.Current.Properties.Add("CustomerId", 0);
            }

            if ((int)App.Current.Properties["CustomerId"] == 0)
            {
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
                    PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
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
                        PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
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
                            PopupNavigation.Instance.PushAsync(new Error_popup(ex.Message));
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


                if (pagename != null)
                {
                    if (pagename == "ViewReservation")
                    {
                        MainPage = new NavigationPage(new ViewReservation(Convert.ToInt32(data)));
                    }
                }
                else
                {
                    MainPage = new NavigationPage(new WelcomPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new WelcomPage());
            }







           

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("6c941890-4bb2-4f03-8adb-36ba3eb26b5d", typeof(Push));

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
