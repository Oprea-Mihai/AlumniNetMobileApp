using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
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
            _getData = new GetData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _photoPickerService = DependencyService.Resolve<IPhotoPickerService>();
            IsImageVisible = false;
            IsRemoveButtonVisible = false;
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

        [ObservableProperty]
        bool _isImageVisible;

        [ObservableProperty]
        bool _isRemoveButtonVisible;

        #endregion

        #region Commands

        [RelayCommand]
        public async void OpenPicker()
        {

            var file = await _photoPickerService.GetImageStreamAsync();
            if (file != null)
            {
                SelectedImage = ImageSource.FromStream(() => file);
                IsImageVisible = true;
                IsRemoveButtonVisible = true;
            }
        }

        [RelayCommand]
        public void RemovePicture()
        {
            IsRemoveButtonVisible=false;
            SelectedImage = null;
            IsImageVisible=false;
        }

        [RelayCommand]
        public async void CreatePost()
        {
            _manageData.SetStrategy(new CreateData());
            string token=await _authenticationService.GetCurrentTokenAsync();
            string json = JsonConvert.SerializeObject(_programToUpdate);

            await _manageData.GetDataAndDeserializeIt<string>($"", "", token);
        }
        #endregion
    }
}
