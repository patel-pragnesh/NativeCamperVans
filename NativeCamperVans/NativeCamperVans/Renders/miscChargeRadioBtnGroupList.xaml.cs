using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NativeCamperVansModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NativeCamperVans.Renders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class miscChargeRadioBtnGroupList : ContentView
    {
        public ObservableCollection<MisChargeOption> itemNames { get; set; }
        
        public miscChargeRadioBtnGroupList()
        {
            InitializeComponent();
            BindingContext = new mslMainPageViewModel(itemNames);

        }

        Model previousModel;
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (previousModel != null)
            {
                previousModel.IsSelected = false;
            }
            Model currentModel = ((CheckBox)sender).BindingContext as Model;
            previousModel = currentModel;

            if (currentModel.IsSelected)
            {
                var viewModel = BindingContext as mslMainPageViewModel;
                int index = viewModel.Items.IndexOf(currentModel);
            }

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (previousModel != null)
            {
                previousModel.IsSelected = false;
            }
            Model currentModel = e.SelectedItem as Model;
            currentModel.IsSelected = true;
            previousModel = currentModel;
        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {

        }
    }

    public class mslMainPageViewModel
    {
        private ObservableCollection<MisChargeOption> itemNames;
        private ObservableCollection<string> itemValues;

        public ObservableCollection<Model> Items { set; get; }
        public mslMainPageViewModel()
        {
            ObservableCollection<Model> list = new ObservableCollection<Model>();
            for (int i = 0; i < 3; i++)
            {
                list.Add(new Model { IsSelected = false });
            }
            Items = list;
        }

        public mslMainPageViewModel(ObservableCollection<MisChargeOption> itemNames)
        {
            this.itemNames = itemNames;

            ObservableCollection<Model> list = new ObservableCollection<Model>();
            foreach (MisChargeOption s in itemNames)
            {
                list.Add(new Model() { IsSelected = false, Name = s.Name, MiscValue =((decimal) s.Value).ToString("0.00") });
            }


            Items = list;
        }
    }

    public class Model : INotifyPropertyChanged
    {
        bool isSelected;
        string name;
        string miscValue;
        public bool IsSelected
        {
            set
            {
                isSelected = value;
                onPropertyChanged();
            }
            get => isSelected;
        }

        public string Name
        {
            set
            {
                name = value;
                onPropertyChanged();
            }
            get => name;
        }

        public string MiscValue
        {
            set
            {
                miscValue = value;
                onPropertyChanged();
            }
            get => miscValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}