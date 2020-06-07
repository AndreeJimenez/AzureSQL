using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AzureSQL.Models;
using AzureSQL.Views;
using AzureSQL.Services;

namespace AzureSQL.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        static ItemsViewModel _instance;

        Command _addCommand;
        public Command AddCommand => _addCommand ?? (_addCommand = new Command(AddAction));

        Command _selectCommand;
        public Command SelectCommand => _selectCommand ?? (_selectCommand = new Command(SelectAction));

        DriverModel driverSelected;
        public DriverModel DriverSelected
        {
            get => driverSelected;
            set => SetProperty(ref driverSelected, value);
        }

        ObservableCollection<DriverModel> drivers;
        public ObservableCollection<DriverModel> Drivers 
        { 
            get => drivers; 
            set => SetProperty(ref drivers, value); 
        }

        public Command LoadDriversCommand { get; set; }

        public ItemsViewModel()
        {
            _instance = this;
            Title = "Drivers";
            Drivers = new ObservableCollection<DriverModel>();
            LoadDriversCommand = new Command(ExecuteLoadDriversCommand);

            /*MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Drivers.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });*/
            ExecuteLoadDriversCommand();
        }

        public static ItemsViewModel GetInstance()
        {
            if (_instance == null) _instance = new ItemsViewModel();
            return _instance;
        }

        private void AddAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new DriverDetailPage());
        }

        private void SelectAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new DriverDetailPage(DriverSelected));
        }

        public async void ExecuteLoadDriversCommand()
        {
            try
            {
                IsBusy = true;
                Drivers.Clear();
                ApiResponse response = await new ApiService().GetDataAsync<DriverModel>("driver"); //DataStore.GetItemsAsync(true);
                if (response != null && response.Result != null)
                {
                    Debug.WriteLine("response.result: " + response.Result.ToString());
                    Drivers = (ObservableCollection<DriverModel>)response.Result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}