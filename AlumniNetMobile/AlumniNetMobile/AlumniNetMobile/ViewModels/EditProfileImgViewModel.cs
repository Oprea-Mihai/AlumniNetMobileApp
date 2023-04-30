using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class EditProfileImgViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors

        public EditProfileImgViewModel(string imageKey)
        {
            _isDeleteEnabled=imageKey!=""?true:false;
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _photoPickerService = DependencyService.Resolve<IPhotoPickerService>();
            _imageKey = imageKey;
        }

        #endregion

        #region Private fields

        private string _imageKey;
        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;
        private IPhotoPickerService _photoPickerService;

        #endregion

        #region Private methods

        [RelayCommand]
        public async void DeletePicture()
        {
            if (_imageKey == "")
                return;
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new DeleteData());
            await _manageData.GetDataAndDeserializeIt<string>($"Profile/DeleteProfilePictureByUserId", "", token);
            MessagingCenter.Send<object>(this, "ProfilePictureChanged");
        }

        [RelayCommand]
        public async void ChangePicture()
        {
            var file = await _photoPickerService.GetImageStreamAsync();
            string token = await _authenticationService.GetCurrentTokenAsync();
            if (file != null)
            {
                UpdateData updateData = new UpdateData();
                await updateData.ManageStreamData($"Profile/UpdateProfilePictureByUserId", file, token);
                MessagingCenter.Send<object>(this, "ProfilePictureChanged");
            }
        }



        #endregion

        #region Observables
        [ObservableProperty]
        bool _isDeleteEnabled;
        #endregion

        #region Commands
        #endregion
    }
}
