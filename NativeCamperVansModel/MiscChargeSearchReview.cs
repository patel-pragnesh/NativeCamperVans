using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NativeCamperVansModel
{
    public class MiscChargeSearchReview : INotifyPropertyChanged
    {
        public int LocationMiscChargeID { get; set; }
        public int VehicleTypeId { get; set; }
        public int MiscChargeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CalculationType { get; set; }
        public decimal Value { get; set; }
        public decimal TotalValue { get; set; }
        public int MisChargeCode { get; set; }
        public int MiscChargeCode { get; set; }
        public bool IsOptional { get; set; }
        public int Unit { get; set; }
        public string PrintValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OldStartDateString { get; set; }
        public string OldEndDateString { get; set; }
        public string StartDateString { get; set; }
        public string CreateddateString { get; set; }
        public string EndDateString { get; set; }
        public bool isQuantity { get; set; }
        public int Quantity { get; set; }

        public bool IsSelected { get; set; }

        public bool IsDeleted { get; set; }

        public bool TaxNotAvailable { get; set; }

        public bool isFreeze { get; set; }
        public bool isAlreadySelected { get; set; }
        public bool IsDeductible { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string VehicleType { get; set; }
        public string GLcode { get; set; }
        public List<MiscChargeTaxModel> MiscTaxList { get; set; }
        public bool IsTaxable { get; set; }
        public string LocationTaxIds { get; set; }
        public bool IsChargeByPeriod { get; set; }
        public decimal Totaltax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? AmountRemaining { get; set; }
        [Serializable]
        public enum CalcType
        {
            Fixed = 1,
            Percentage = 2,
            Perday = 3,
            Replace = 4
        }

        public string ViewString { get; set; }

        public bool IsMantatory { get { return !IsOptional; } }

        public decimal price
        {
            get;
            //{
            // //if (IsQuantity)
            // //{
            // // return Value * Unit;
            // //}
            // //else
            // //{
            // // return Value;
            // //}
            //}
            set;

        }

        public decimal _price
        {
            get => price;
            //{
            // //if (IsQuantity)
            // //{
            // // return Value * Unit;
            // //}
            // //else
            // //{
            // // return Value;
            // //}
            //}
            set
            {
                if (isQuantity)
                {
                    price= Value * Quantity;
                    OnPropertyChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("_price"));
                }
                else
                {
                    price = Value;
                    PropertyChanged(this, new PropertyChangedEventArgs("_price"));
                }
            }

        }

        public int _Quantity
        {
            get => Quantity;
            set
            {
                if (Quantity != value)
                {
                    Quantity = value;
                    OnPropertyChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("_Quantity"));
                }
            }
        }

        public List<MisChargeOption> MisChargeOptionList { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}