using AzureSQL.Models;
using AzureSQL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzureSQL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DriverDetailPage : ContentPage
    {
        public DriverDetailPage()
        {
            InitializeComponent();

            BindingContext = new DriverDetailViewModel();
        }

        public DriverDetailPage(DriverModel driver)
        {
            InitializeComponent();

            BindingContext = new DriverDetailViewModel(driver);
        }
    }
}