﻿using NativeCamperVans.Popups;
using NativeCamperVans.Renders;
using NativeCamperVansController;
using NativeCamperVansModel;
using NativeCamperVansModel.AccessModels;
using Plugin.InputKit.Shared.Controls;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NativeCamperVans.Views
{
    public partial class VehicleRates : ContentPage
    {
        //MisChargeFilter misChargeFilter;
        GetMischargeSearchDetailsMobileRequest misChargeRequest;
        //TaxFilter taxFilter;
        GetTaxMobileListRequest taxRequest;
        GetPromotionMobileRequest promotionMobileRequest;
        GetPromotionMobileResponse promotionMobileResponse;
        List<MiscChargeSearchReview> misChargeResults;
        List<MiscChargeSearchReview> misChargeResultsNonSelectable;
        List<MiscChargeSearchReview> misChargeResultsSelectable;
        List<MiscChargeSearchReview> misChargeResultsSelectableDeducible;
        List<MiscChargeSearchReview> misChargeResultsSelectableDeducibleThree;
        List<MiscChargeSearchReview> misChargeResultsSelectableDeducibleFour;
        List<MiscChargeSearchReview> misChargeResultsSelectableDeducibleFive;
        GetMischargeSearchDetailsMobileResponse misChargeResponse;
        List<LocationTaxModel> taxResults;
        GetTaxMobileListResponse taxResponse;
        ReservationController reservationController;
        string token;
        private ReservationView reservationView;
        private VehicleViewByTypeForMobile selectedVehicle;
        Dictionary<int, string> selectedDeductibleMiscValues;



        public VehicleRates(ReservationView reservationView, VehicleViewByTypeForMobile selectedVehicle)
        {
            InitializeComponent();
            this.reservationView = reservationView;
            //misChargeFilter = new MisChargeFilter();
            misChargeRequest = new GetMischargeSearchDetailsMobileRequest();
            //taxFilter = new TaxFilter();
            taxRequest = new GetTaxMobileListRequest();
            promotionMobileRequest = new GetPromotionMobileRequest();
            //misChargeFilter.LocationId =(int) reservationView.StartLocationId;
            //misChargeFilter.VehicleTypeId =(int) reservationView.VehicleTypeID;
            misChargeRequest.LocationId = (int)reservationView.StartLocationId;
            misChargeRequest.VehicleTypeId = (int)reservationView.VehicleTypeID;
            //taxFilter.LocationId = (int)reservationView.StartLocationId;
            taxRequest.LocationId = (int)reservationView.StartLocationId;
            promotionMobileResponse = null;
            misChargeResults = null;
            misChargeResponse = null;
            taxResults = null;
            taxResponse = null;
            reservationController = new ReservationController();
            token = App.Current.Properties["currentToken"].ToString();
            this.selectedVehicle = selectedVehicle;
            startDateLabel.Text = ((DateTime)reservationView.StartDate).ToString("ddd MM/dd/yyyy");
            endDateLabel.Text = ((DateTime)reservationView.EndDate).ToString("ddd MM/dd/yyyy");
            startTimeLabel.Text = ((DateTime)reservationView.StartDate).ToString("hh:mm tt");
            endTimeLabel.Text = ((DateTime)reservationView.EndDate).ToString("hh:mm tt");
            if (selectedVehicle.VehicleTypeImageUrl != null)
            {
                vehilcleTypeImage.Source = ImageSource.FromUri(new Uri(selectedVehicle.VehicleTypeImageUrl));
            }
            vehicleSampleLabel.Text = selectedVehicle.sample;
            vehilcleTypeLabel.Text = selectedVehicle.VehicleType;
            priceLabel.Text = "$ " + selectedVehicle.RateDetail.RateTotal.ToString();
            selectedDeductibleMiscValues = new Dictionary<int, string>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var assembly = typeof(VehicleRates);
            //if ((int)App.Current.Properties["CustomerId"] == 0)
            //{
            //    loginIcon.IconImageSource = ImageSource.FromResource("NativeCamperVans.Assets.LogInTool.png", assembly);

            //}
            //else
            //{
            //    loginIcon.IconImageSource = ImageSource.FromResource("NativeCamperVans.Assets.logOutTool.png", assembly);
            //}

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                //if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() == typeof(ErrorWithClosePagePopup))
                //{
                await PopupNavigation.Instance.PopAllAsync();
                //}
            }

            bool busy = false;
            if (!busy)
            {
                try
                {
                    busy = true;
                    await PopupNavigation.Instance.PushAsync(new LoadingPopup("Getting vehicles rates..."));

                    await Task.Run(() =>
                    {
                        try
                        {
                            //misChargeResults = reservationController.getMisCharge(misChargeFilter,token);
                            misChargeResponse = reservationController.getMisChargeMobile(misChargeRequest, token);
                            //taxResults = reservationController.getTax(taxFilter,token);
                            taxResponse = reservationController.GetTaxMobileList(taxRequest, token);



                        }
                        catch (Exception ex)
                        {
                            PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
                        }
                    });
                }
                finally
                {
                    busy = false;
                    if (PopupNavigation.Instance.PopupStack.Count == 1)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                    }
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                    }


                    if (taxResponse.message.ErrorCode == "200")
                    {
                        taxResults = taxResponse.LocationTaxResult;
                        foreach (LocationTaxModel t in taxResults)
                        {
                            if (t.IsOption)
                            {
                                t.IsSelected = false;
                                if (reservationView.TaxList2 != null)
                                {
                                    if (reservationView.TaxList2.Count > 0)
                                    {
                                        foreach (LocationTaxModel ltm in reservationView.TaxList2)
                                        {
                                            if (ltm.TaxId == t.TaxId)
                                            {
                                                t.IsSelected = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                t.IsSelected = true;
                            }

                        }
                    };
                    if (taxResponse.message.ErrorCode != "200")
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup(taxResponse.message.ErrorMessage));
                    };


                    if (misChargeResponse.message.ErrorCode == "200")
                    {
                        misChargeResults = misChargeResponse.MischargeResultList;
                        misChargeResultsSelectable = new List<MiscChargeSearchReview>();
                        misChargeResultsNonSelectable = new List<MiscChargeSearchReview>();
                        misChargeResultsSelectableDeducible = new List<MiscChargeSearchReview>();
                        misChargeResultsSelectableDeducibleThree = new List<MiscChargeSearchReview>();
                        misChargeResultsSelectableDeducibleFour = new List<MiscChargeSearchReview>();
                        misChargeResultsSelectableDeducibleFive = new List<MiscChargeSearchReview>();
                        if (misChargeResults != null)
                        {
                            foreach (MiscChargeSearchReview m in misChargeResults)
                            {
                                switch (m.CalculationType)
                                {
                                    case "Perday":
                                        if (m.IsDeductible)
                                        {
                                            m.ViewString = "( " + m.CalculationType + " ) ";
                                            m.price = (decimal)m.Value * (decimal)selectedVehicle.RateDetail.TotalDays;
                                            break;
                                        }
                                        else
                                        {
                                            m.ViewString = "( " + m.CalculationType + " $" + m.Value + " ) x " + selectedVehicle.RateDetail.TotalDays;
                                            m.price = (decimal)m.Value * (decimal)selectedVehicle.RateDetail.TotalDays;
                                            break;
                                        }

                                    case "Fixed":
                                        if (m.IsDeductible)
                                        {
                                            m.ViewString = "( " + m.CalculationType + " )";
                                            m.price = m.Value;
                                            break;
                                        }
                                        else
                                        {
                                            m.ViewString = "( " + m.CalculationType + " $" + m.Value + " )";
                                            m.price = m.Value;
                                            break;
                                        }

                                }

                                //if (m.IsQuantity) { m.price=(decimal)m.Value *(decimal)m.Unit; }
                                //else
                                //{
                                // m.price=m.Value;
                                //}

                                if (!m.IsOptional)
                                {
                                    m.IsSelected = false;
                                    if (reservationView.MiscList2 != null)
                                    {
                                        if (reservationView.MiscList2.Count > 0)
                                        {
                                            foreach (MiscChargeSearchReview msv in reservationView.MiscList2)
                                            {
                                                if (msv.MiscChargeID == m.MiscChargeID)
                                                {
                                                    m.IsSelected = true;
                                                }
                                            }
                                        }
                                    }
                                    if (m.IsDeductible)
                                    {
                                        if (m.MisChargeOptionList.Count == 2)
                                        {
                                            misChargeResultsSelectableDeducible.Add(m);
                                        }
                                        if (m.MisChargeOptionList.Count == 3)
                                        {
                                            misChargeResultsSelectableDeducibleThree.Add(m);
                                        }
                                        if (m.MisChargeOptionList.Count == 4)
                                        {
                                            misChargeResultsSelectableDeducibleFour.Add(m);
                                        }
                                        if (m.MisChargeOptionList.Count == 5)
                                        {
                                            misChargeResultsSelectableDeducibleFive.Add(m);
                                        }

                                        if (selectedDeductibleMiscValues.ContainsKey(m.MiscChargeID))
                                        {
                                            foreach (MisChargeOption mscp in m.MisChargeOptionList)
                                            {
                                                if (mscp.Value.ToString() == selectedDeductibleMiscValues[m.MiscChargeID])
                                                {
                                                    mscp.IsSelect = true;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        misChargeResultsSelectable.Add(m);
                                    }
                                }
                                else
                                {
                                    m.IsSelected = true;

                                    if (m.IsDeductible)
                                    {
                                        if (m.MisChargeOptionList.Count == 2)
                                        {
                                            misChargeResultsSelectableDeducible.Add(m);
                                        }
                                        if (m.MisChargeOptionList.Count == 3)
                                        {
                                            misChargeResultsSelectableDeducibleThree.Add(m);
                                        }
                                        if (m.MisChargeOptionList.Count == 4)
                                        {
                                            misChargeResultsSelectableDeducibleFour.Add(m);
                                        }
                                        if (m.MisChargeOptionList.Count == 5)
                                        {
                                            misChargeResultsSelectableDeducibleFive.Add(m);
                                        }

                                        if (selectedDeductibleMiscValues.ContainsKey(m.MiscChargeID))
                                        {
                                            foreach (MisChargeOption mscp in m.MisChargeOptionList)
                                            {
                                                if (mscp.Value.ToString() == selectedDeductibleMiscValues[m.MiscChargeID])
                                                {
                                                    mscp.IsSelect = true;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        misChargeResultsNonSelectable.Add(m);
                                    }
                                }
                            }
                        }
                    };
                    if (misChargeResponse.message.ErrorCode != "200")
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup(misChargeResponse.message.ErrorMessage));
                    };


                    if (misChargeResultsNonSelectable.Count() > 0)
                    {
                        RateList.ItemsSource = misChargeResultsNonSelectable;
                        RateList.HeightRequest = misChargeResultsNonSelectable.Count() * 65;
                    }
                    if (misChargeResultsNonSelectable.Count() == 0)
                    {
                        RateList.IsVisible = false;
                    }


                    if (misChargeResultsSelectable.Count() > 0)
                    {
                        RateListSelectLabel.ItemsSource = misChargeResultsSelectable;
                        RateListSelectLabel.HeightRequest = misChargeResultsSelectable.Count() * 80;
                    }
                    if (misChargeResultsSelectable.Count() == 0)
                    {
                        RateListSelectLabel.IsVisible = false;
                    }

                    if (taxResults.Count() > 0)
                    {
                        taxList.ItemsSource = taxResults;
                        taxList.HeightRequest = taxResults.Count() * 65;
                    }
                    if (taxResults.Count() == 0)
                    {
                        taxList.IsVisible = false;
                        taxHeadingLabel.IsVisible = false;
                    }



                    if (misChargeResultsSelectableDeducible.Count() > 0)
                    {
                        RateListSelectLabelDeducible.ItemsSource = misChargeResultsSelectableDeducible;
                        RateListSelectLabelDeducible.HeightRequest = misChargeResultsSelectableDeducible.Count * 140;
                    }
                    if (misChargeResultsSelectableDeducible.Count() == 0)
                    {
                        RateListSelectLabelDeducible.IsVisible = false;
                    }

                    if (misChargeResultsSelectableDeducibleThree.Count() > 0)
                    {
                        RateListSelectLabelDeducibleThree.ItemsSource = misChargeResultsSelectableDeducibleThree;
                        RateListSelectLabelDeducibleThree.HeightRequest = misChargeResultsSelectableDeducibleThree.Count * 140;
                    }
                    if (misChargeResultsSelectableDeducibleThree.Count() == 0)
                    {
                        RateListSelectLabelDeducibleThree.IsVisible = false;
                    }

                    if (misChargeResultsSelectableDeducibleFour.Count() > 0)
                    {
                        RateListSelectLabelDeducibleFour.ItemsSource = misChargeResultsSelectableDeducibleFour;
                        RateListSelectLabelDeducibleFour.HeightRequest = misChargeResultsSelectableDeducibleFour.Count * 140;
                    }
                    if (misChargeResultsSelectableDeducibleFour.Count() == 0)
                    {
                        RateListSelectLabelDeducibleFour.IsVisible = false;
                    }


                    if (misChargeResultsSelectableDeducibleFive.Count() > 0)
                    {
                        RateListSelectLabelDeducibleFive.ItemsSource = misChargeResultsSelectableDeducibleFive;
                        RateListSelectLabelDeducibleFive.HeightRequest = misChargeResultsSelectableDeducibleFive.Count * 140;
                    }
                    if (misChargeResultsSelectableDeducibleFive.Count() == 0)
                    {
                        RateListSelectLabelDeducibleFive.IsVisible = false;
                    }


                }

            }



        }

        //private async void LoginIcon_Clicked(object sender, EventArgs e)
        //{
        //    var assembly = typeof(VehicleRates);
        //    if ((int)App.Current.Properties["CustomerId"] == 0)
        //    {

        //        loginIcon.IconImageSource = ImageSource.FromResource("NativeCamperVans.Assets.logOutTool.png", assembly);
        //        await Navigation.PushAsync(new LoginPage());

        //    }
        //    else
        //    {
        //        bool logout = await DisplayAlert("Alert", "Are you sure want to logout", "Yes", "No");
        //        if (logout)
        //        {
        //            App.Current.Properties["CustomerId"] = 0;
        //            loginIcon.IconImageSource = ImageSource.FromResource("NativeCamperVans.Assets.LogInTool.png", assembly);
        //            await Navigation.PushAsync(new WelcomPage());
        //        }
        //    }
        //}

        private void BacKBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void NxtBtn_Clicked(object sender, EventArgs e)
        {
            List<MiscChargeSearchReview> miscChargeSearchReviews = new List<MiscChargeSearchReview>();

            if (misChargeResultsNonSelectable.Count() > 0)
            {
                List<MiscChargeSearchReview> itemListMis = RateList.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMis)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            if (misChargeResultsSelectable.Count() > 0)
            {
                List<MiscChargeSearchReview> itemListMis2 = RateListSelectLabel.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMis2)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            if (misChargeResultsSelectableDeducible.Count > 0)
            {
                List<MiscChargeSearchReview> itemListMisde2 = RateListSelectLabelDeducible.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMisde2)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        if (selectedDeductibleMiscValues != null)
                        {
                            if (selectedDeductibleMiscValues.ContainsKey(msr.MiscChargeID))
                            {
                                msr.Value = decimal.Parse(selectedDeductibleMiscValues[msr.MiscChargeID]);
                            }
                        }
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            if (misChargeResultsSelectableDeducibleThree.Count > 0)
            {
                List<MiscChargeSearchReview> itemListMisde3 = RateListSelectLabelDeducibleThree.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMisde3)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        if (selectedDeductibleMiscValues != null)
                        {
                            if (selectedDeductibleMiscValues.ContainsKey(msr.MiscChargeID))
                            {
                                msr.Value = decimal.Parse(selectedDeductibleMiscValues[msr.MiscChargeID]);
                            }
                        }
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            if (misChargeResultsSelectableDeducibleFour.Count > 0)
            {
                List<MiscChargeSearchReview> itemListMisde4 = RateListSelectLabelDeducibleFour.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMisde4)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        if (selectedDeductibleMiscValues != null)
                        {
                            if (selectedDeductibleMiscValues.ContainsKey(msr.MiscChargeID))
                            {
                                msr.Value = decimal.Parse(selectedDeductibleMiscValues[msr.MiscChargeID]);
                            }
                        }
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }


            if (misChargeResultsSelectableDeducibleFive.Count > 0)
            {
                List<MiscChargeSearchReview> itemListMisde5 = RateListSelectLabelDeducibleFive.ItemsSource as List<MiscChargeSearchReview>;
                foreach (MiscChargeSearchReview msr in itemListMisde5)
                {

                    msr.StartDate = (DateTime)reservationView.StartDate;
                    msr.EndDate = (DateTime)reservationView.EndDate;
                    msr.StartDateString = reservationView.StartDateStr;
                    msr.EndDateString = reservationView.EndDateStr;
                    if (msr.IsSelected)
                    {
                        if (selectedDeductibleMiscValues != null)
                        {
                            if (selectedDeductibleMiscValues.ContainsKey(msr.MiscChargeID))
                            {
                                msr.Value = decimal.Parse(selectedDeductibleMiscValues[msr.MiscChargeID]);
                            }
                        }
                        miscChargeSearchReviews.Add(msr);
                    }

                }
            }

            reservationView.MiscList2 = miscChargeSearchReviews;


            if (taxResults.Count() > 0)
            {
                List<LocationTaxModel> locationTaxModels = new List<LocationTaxModel>();
                List<LocationTaxModel> itemsourceTax = taxList.ItemsSource as List<LocationTaxModel>;
                foreach (LocationTaxModel ltm in itemsourceTax)
                {
                    LocationTaxModel locationTaxModel = new LocationTaxModel();
                    locationTaxModel.LocationId = ltm.LocationId;
                    locationTaxModel.TaxId = ltm.TaxId;
                    locationTaxModel.Name = ltm.Name;
                    locationTaxModel.Description = ltm.Description;
                    locationTaxModel.Value = ltm.Value;
                    locationTaxModel.LocationName = ltm.LocationName;
                    locationTaxModel.IsSelected = ltm.IsSelected;
                    if (locationTaxModel.IsSelected)
                    {
                        locationTaxModels.Add(locationTaxModel);
                    }

                }
                reservationView.TaxList2 = locationTaxModels;
            }
            Navigation.PushAsync(new SummaryOfChargesPage(reservationView, selectedVehicle));
        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {

        }
        private async void PromoBtn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(promoCodeEntry.Text))
            {
                promotionMobileRequest.PromotionCode = promoCodeEntry.Text;
                promotionMobileRequest.LocationId = (int)reservationView.StartLocationId;
                promotionMobileRequest.VehicleTypeId = (int)reservationView.VehicleTypeID;

                bool busy = false;
                if (!busy)
                {
                    try
                    {
                        busy = true;
                        await PopupNavigation.Instance.PushAsync(new LoadingPopup("Checking for promotion..."));

                        await Task.Run(() =>
                        {
                            try
                            {
                                promotionMobileResponse = reservationController.checkPromotion(promotionMobileRequest, token);
                            }
                            catch (Exception ex)
                            {
                                PopupNavigation.Instance.PushAsync(new ErrorWithClosePagePopup(ex.Message));
                            }
                        });
                    }
                    finally
                    {
                        busy = false;
                        if (PopupNavigation.Instance.PopupStack.Count == 1)
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                        }
                        if (PopupNavigation.Instance.PopupStack.Count > 1)
                        {
                            if (PopupNavigation.Instance.PopupStack[PopupNavigation.Instance.PopupStack.Count - 1].GetType() != typeof(ErrorWithClosePagePopup))
                            {
                                await PopupNavigation.Instance.PopAllAsync();
                            }
                        }


                    }
                    if (promotionMobileResponse.promotion == null)
                    {
                        await PopupNavigation.Instance.PushAsync(new Error_popup("Invalid promo code"));
                    }
                    else
                    {
                        await PopupNavigation.Instance.PushAsync(new SuccessPopUp("Promo code has been applied successfully!"));
                        reservationView.PromotionCode = promoCodeEntry.Text;
                        //if(reservationView.PromotionList== null)
                        //{
                        reservationView.PromotionList = new List<PromotionItem>();
                        //}

                        reservationView.PromotionList.Add(new PromotionItem() { PromotionID = promotionMobileResponse.PromResult.PromotionID, PromotionDiscount = (decimal)promotionMobileResponse.PromResult.DiscountValue });
                    }
                }
            }
        }

        private void TaxCheckbox_CheckChanged(object sender, EventArgs e)
        {

        }

        private void ExtStepper_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var obj = (ExtStepper)sender;
            var objGrid = (Grid)obj.Parent;
            var viewCell = (ExtendedViewCell)objGrid.Parent;

            //var data = obj.BindingContext as MischargeResultMobile;
            //foreach(MischargeResultMobile msrm  in misChargeResults)
            //{
            //    if(data.MiscChargeID== msrm.MiscChargeID)
            //    {
            //        msrm.Unit = obj.Value;
            //    }
            //}
        }

        private void ExtStepper_AddClicked(object sender, EventArgs e)
        {
            var obj = (Button)sender;
            var objGrid = (Grid)obj.Parent;
            var viewCell = (ExtendedViewCell)objGrid.Parent;

            List<MiscChargeSearchReview> newList = misChargeResultsSelectable;

            var data = objGrid.BindingContext as MiscChargeSearchReview;
            foreach (MiscChargeSearchReview msrm in newList)
            {
                if (data.MiscChargeID == msrm.MiscChargeID)
                {
                    msrm._Quantity = msrm.Quantity + 1;
                    msrm._price = msrm.Value * msrm._Quantity;

                }
            }
            RateListSelectLabel.ItemsSource = null;
            RateListSelectLabel.ItemsSource = newList;
            RateListSelectLabel.HeightRequest = newList.Count() * 80;
        }

        private void ExtStepper_SubClicked(object sender, EventArgs e)
        {
            var obj = (Button)sender;
            var objGrid = (Grid)obj.Parent;
            var viewCell = (ExtendedViewCell)objGrid.Parent;

            List<MiscChargeSearchReview> newList = misChargeResultsSelectable;

            var data = viewCell.BindingContext as MiscChargeSearchReview;
            foreach (MiscChargeSearchReview msrm in newList)
            {
                if (data.MiscChargeID == msrm.MiscChargeID)
                {
                    if (msrm.Quantity > 0)
                    {
                        msrm._Quantity = msrm.Quantity - 1;
                        msrm._price = msrm.Value * msrm._Quantity;
                    }

                }
            }

            RateListSelectLabel.ItemsSource = null;
            RateListSelectLabel.ItemsSource = newList;
            RateListSelectLabel.HeightRequest = newList.Count() * 80;
        }

        private void descriptionBtn_Tapped(object sender, EventArgs e)
        {

        }

        private void btnDecrease_Clicked(object sender, EventArgs e)
        {
            var obj = (Button)sender;
            var objGrid = (Grid)obj.Parent;
            //            var viewCell = (ExtendedViewCell)objGrid.Parent;

            List<MiscChargeSearchReview> newList = misChargeResultsSelectable;

            var data = objGrid.BindingContext as MiscChargeSearchReview;
            foreach (MiscChargeSearchReview msrm in newList)
            {
                if (data.MiscChargeID == msrm.MiscChargeID)
                {
                    if (msrm.Quantity > 0)
                    {
                        msrm._Quantity = msrm.Quantity - 1;
                        msrm._price = msrm.Value * msrm._Quantity;
                    }

                }
            }

            RateListSelectLabel.ItemsSource = null;
            RateListSelectLabel.ItemsSource = newList;
            RateListSelectLabel.HeightRequest = newList.Count() * 80;
        }

        private void btnincrease_Clicked(object sender, EventArgs e)
        {
            var obj = (Button)sender;
            var objGrid = (Grid)obj.Parent;
            //         var viewCell = (ExtendedViewCell)objGrid.Parent;

            List<MiscChargeSearchReview> newList = misChargeResultsSelectable;

            var data = objGrid.BindingContext as MiscChargeSearchReview;
            foreach (MiscChargeSearchReview msrm in newList)
            {
                if (data.MiscChargeID == msrm.MiscChargeID)
                {
                    msrm._Quantity = msrm.Quantity + 1;
                    msrm._price = msrm.Value * msrm._Quantity;

                }
            }
            RateListSelectLabel.ItemsSource = null;
            RateListSelectLabel.ItemsSource = newList;
            RateListSelectLabel.HeightRequest = newList.Count() * 80;
        }

        private void misDeduRadiaBtnGroup_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void RadioButtonGroupView_SelectedItemChanged(object sender, EventArgs e)
        {

            var obj = (RadioButtonGroupView)sender;
            var objGrid = (Grid)obj.Parent;
            MiscChargeSearchReview misChargeOption = objGrid.BindingContext as MiscChargeSearchReview;
            int value = obj.SelectedIndex;

            if (misChargeOption != null)
            {
                decimal val = (decimal)misChargeOption.MisChargeOptionList[value].Value;
                if (selectedDeductibleMiscValues.ContainsKey(misChargeOption.MiscChargeID))
                {
                    selectedDeductibleMiscValues[misChargeOption.MiscChargeID] = val.ToString();
                }
                else
                {
                    selectedDeductibleMiscValues.Add(misChargeOption.MiscChargeID, val.ToString());
                }
            }


            //DisplayAlert("iadh.", value.ToString(), "sdh");
        }
    }
}         