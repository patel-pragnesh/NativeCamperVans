﻿using NativeCamperVans.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NativeCamperVans.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedSuccessfullyPopup : PopupPage
    {
        public SavedSuccessfullyPopup()
        {
            InitializeComponent();
        }

        private async void Okbtn_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                for (var counter = 1; counter < 3; counter++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                }
                if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(LoginPage))
                {
                    await Navigation.PopAsync();
                }
                else if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(WelcomPage))
                {
                    await Navigation.PushAsync(new LoginPage());
                }
                else if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 2].GetType() == typeof(SummaryOfChargesPage))
                {
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
           
           
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}