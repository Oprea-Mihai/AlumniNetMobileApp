using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks.Sources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddPostViewModel : ObservableObject
    {
        #region Constructors

        public AddPostViewModel()
        {
            _manageData = new ManageData();
            _getData=new GetData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _photoPickerService = DependencyService.Resolve<IPhotoPickerService>();
           
        }

        #endregion

        #region Private fields
        private IPhotoPickerService _photoPickerService;
        private ManageData _manageData;
        private GetData _getData;
        private IAuthenticationService _authenticationService;
        #endregion

        #region Private methods
        #endregion

        #region Observables

        [ObservableProperty]
        ImageSource _selectedImage;

        #endregion

        #region Commands

        [RelayCommand]
        public async void OpenPicker()
        {
            //TO UNCOMMENT!!!!!!!!
            //var file = await _photoPickerService.GetImageStreamAsync();
            //SelectedImage = ImageSource.FromStream(() => file);


            //THIS IS JUST A DEMO:
            _manageData.SetStrategy(new GetData());
            Stream file = await _getData.ManageStreamData($"Buckets/GetFileByKey?bucketName=alumni-app-bucket&key=delete.png");
            SelectedImage = ImageSource.FromStream(() => file);
        }

        #endregion
    }
}
