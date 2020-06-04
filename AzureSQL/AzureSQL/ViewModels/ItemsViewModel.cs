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
        Command _addCommand;
        public Command NewCommand => _addCommand ?? (_addCommand = new Command(AddAction));

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

        private void AddAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new DriverDetailPage());
        }

        private void SelectAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new DriverDetailPage(DriverSelected));
        }

        async void ExecuteLoadDriversCommand()
        {
            IsBusy = true;

            try
            {
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