using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AzureSQL.Services;
using AzureSQL.Views;

namespace AzureSQL
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ItemsPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
