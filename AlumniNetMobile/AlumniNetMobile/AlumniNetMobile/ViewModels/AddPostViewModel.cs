using AlumniNetMobile.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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
            _photoPickerService = DependencyService.Resolve<IPhotoPickerService>();
        }

        #endregion

        #region Private fields
        private IPhotoPickerService _photoPickerService;
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
            
            var file = await _photoPickerService.GetImageStreamAsync();
            SelectedImage = ImageSource.FromStream(() => file);
        }

        #endregion
    }
}
