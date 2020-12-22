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
    public partial class SuccessPopUp : PopupPage
    {
        private int v;

        public SuccessPopUp(string content)
        {
            InitializeComponent();
            contentText.Text = content;
            v = 0;
        }

        public SuccessPopUp(string content, int v)
        {
            InitializeComponent();
            contentText.Text = content;
            this.v = v;
        }

        private void Okbtn_Clicked(object sender, EventArgs e)
        {
            if (v == 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            if (v == 1)
            {
                Navigation.PushAsync(new HomePage());
            }
            if (v == 2)
            {
                Navigation.PushAsync(new MyProfile());
            }
        }
    }
}