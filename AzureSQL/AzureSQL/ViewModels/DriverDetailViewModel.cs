using AzureSQL.Models;
using AzureSQL.Services;
using AzureSQL.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AzureSQL.ViewModels
{
    class DriverDetailViewModel : BaseViewModel
    {
        Command _saveCommand;
        public Command SaveCommand => _saveCommand ?? (_saveCommand = new Command(SaveAction));

        Command _deleteCommand;
        public Command DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(DeleteAction));

        Command _GetLocationCommand;
        public Command GetLocationCommand => _GetLocationCommand ?? (_GetLocationCommand = new Command(GetLocationAction));

        Command _TakePictureCommand;
        public Command TakePictureCommand => _TakePictureCommand ?? (_TakePictureCommand = new Command(TakePictureAction));

        Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));

        int _ID;
        public int id
        {
            get => _ID;
            set => SetProperty(ref _ID, value);
        }

        string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        string _Picture;
        public string Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }

        string _Status;
        public string Status
        {
            get => _Status;
            set => SetProperty(ref _Status, value);
        }

        public string _Longitude;
        public string Longitude
        {
            get => _Longitude;
            set => SetProperty(ref _Longitude, value);
        }

        string _Latitude;
        public string Latitude
        {
            get => _Latitude;
            set => SetProperty(ref _Latitude, value);
        }

        public DriverModel _Driver;
        public DriverModel Driver
        {
            get => _Driver;
            set => SetProperty(ref _Driver, value);
        }

        ImageSource _PictureSource;
        public ImageSource PictureSource
        {
            get => _PictureSource;
            set => SetProperty(ref _PictureSource, value);
        }

        public DriverDetailViewModel(){}

        public DriverDetailViewModel(DriverModel driver)
        {
            if (driver != null)
            {
                id = driver.IDDriver;
                Name = driver.Name;
                Status = driver.Status;
                Picture = driver.Picture;
                Latitude = driver.ActualPosition.Latitude;
                Longitude = driver.ActualPosition.Longitude;
            }
        }

        private async void SaveAction()
        {
            IsBusy = true;
            if (id == 0)
            {
                ApiResponse response = await new ApiService().PostDataAsync("driver", new DriverModel
                {
                    Name = this.Name,
                    Status = this.Status,
                    Picture = this.Picture,
                    ActualPosition = new PositionModel
                    {
                        Latitude = this.Latitude,
                        Longitude = this.Longitude
                    }
                });
                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("AzureSQL", "Error creating driver", "Ok");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AzureSQL", response.Message, "Ok");
                    return;
                }
                ItemsViewModel.GetInstance().ExecuteLoadDriversCommand();
                await Application.Current.MainPage.DisplayAlert("AzureSQL", response.Message, "Ok");
            }
            else
            {
                ApiResponse response = await new ApiService().PutDataAsync("driver", this.id, new DriverModel
                {
                    Name = this.Name,
                    Status = this.Status,
                    Picture = this.Picture,
                    ActualPosition = new PositionModel
                    {
                        Latitude = this.Latitude,
                        Longitude = this.Longitude
                    }
                });
                if (response == null)
                {
                    await Application.Current.MainPage.DisplayAlert("AzureSQL", "Error updating driver", "Ok");
                    return;
                }
                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("AzureSQL", response.Message, "Ok");
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("AzureSQL", response.Message, "Ok");
            }
            IsBusy = false;
            ItemsViewModel.GetInstance().ExecuteLoadDriversCommand();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            IsBusy = true;

            ApiResponse response = await new ApiService().DeleteDataAsync("driver", id);
            if (response == null)
            {
                await Application.Current.MainPage.DisplayAlert("AzureSQL", "Error removing driver", "Ok");
                return;
            }
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("AzureSQL", response.Message, "OK");
                return;
            }
            ItemsViewModel.GetInstance().ExecuteLoadDriversCommand();
            await Application.Current.MainPage.DisplayAlert("AzureSQL", response.Message, "Ok");

            IsBusy = false;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void GetLocationAction()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Latitude = location.Latitude.ToString();
                    Longitude = location.Longitude.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void TakePictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            this.Picture = await new ImageService().ConvertImageFileToBase64(file.Path);
            await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            PictureSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream(); //del archivo que ya obtuvo el plugin 
                return stream;
            });
        }

        private async void SelectPictureAction()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await CrossMedia.Current.Initialize();
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Not supported", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (file == null)
                return;

            this.Picture = await new ImageService().ConvertImageFileToBase64(file.Path);

            PictureSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream(); //del archivo que ya obtuvo el plugin 
                return stream;
            });
        }
    }
}