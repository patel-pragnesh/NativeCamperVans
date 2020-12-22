using NativeCamperVans.Views;
using NativeCamperVansController;
using NativeCamperVansModel.AccessModels;
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
    public partial class Error_popup : PopupPage
    {
        private bool v;
        private CancelReservationMobileRequest cancelReservationMobileRequest;
        private string token;
        private int v1;

        public Error_popup(string content)
        {
            InitializeComponent();
            contentText.Text = content;
            v = false;
            v1 = 0;
        }

        public Error_popup(string content, bool v)
        {
            InitializeComponent();
            contentText.Text = content;
            this.v = v;
        }

        public Error_popup(string content, CancelReservationMobileRequest cancelReservationMobileRequest, string token)
        {
            InitializeComponent();
            contentText.Text = content;
            this.cancelReservationMobileRequest = cancelReservationMobileRequest;
            this.token = token;
            Okbtn.IsVisible = false;
            canReserGrid.IsVisible = true;
        }

        public Error_popup(string content, int v1)
        {
            InitializeComponent();
            contentText.Text = content;
            v = false;
            this.v1 = v1;
        }

        private void Okbtn_Clicked(object sender, EventArgs e)
        {
            if (v)
            {
                Navigation.PushAsync(new HomePage());
            }
            else if (v1 == 1)
            {
                Navigation.PushAsync(new VehicleDetailPage());
            }
            else if (v1 == 2)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            else
            {
                PopupNavigation.Instance.PopAsync();
            }
        }

        private async void Okbtng_Clicked(object sender, EventArgs e)
        {
            ReservationController reservationController = new ReservationController();
            CancelReservationMobileResponse response = null;



            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Cancelling Reservation"));
                    await Task.Run(() =>
                    {

                        response = reservationController.cancelReservation(cancelReservationMobileRequest, token);
                    });
                }
                finally
                {
                    busy = false;
                    

                    if (response.ReservationNumber != null)
                    {
                        MessagingCenter.Send(this,"reservationCancelled");
                        await Navigation.PushAsync(new HomePage());

                    }
                    await PopupNavigation.Instance.PopAllAsync();

                }
            }

        }

        private void noBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}