using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AzureSQL.Models;
using AzureSQL.Views;
using AzureSQL.ViewModels;

namespace AzureSQL.Views
{
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Drivers.Count == 0)
                viewModel.IsBusy = true;
        }

        /*async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var driver = (DriverModel)layout.BindingContext;
            await Navigation.PushAsync(new DriverDetailPage(new DriverDetailViewModel(driver)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }*/
    }
}