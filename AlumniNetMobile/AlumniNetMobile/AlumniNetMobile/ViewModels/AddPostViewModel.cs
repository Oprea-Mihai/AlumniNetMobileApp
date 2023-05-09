using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
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
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _photoPickerService = DependencyService.Resolve<IPhotoPickerService>();
            IsImageVisible = false;
            IsRemoveButtonVisible = false;
            PostText = string.Empty;
        }

        #endregion

        #region Private fields
        private IPhotoPickerService _photoPickerService;
        private ManageData _manageData;
        private Stream _selectedFile;
        private MemoryStream _memoryStream;
        private IAuthenticationService _authenticationService;
        #endregion

        #region Private methods
        #endregion

        #region Observables

        [ObservableProperty]
        ImageSource _selectedImage;

        [ObservableProperty]
        string _postText;

        [ObservableProperty]
        bool _isImageVisible;

        [ObservableProperty]
        bool _isRemoveButtonVisible;

        #endregion

        #region Commands

        [RelayCommand]
        public async void OpenPicker()
        {

            _selectedFile = await _photoPickerService.GetImageStreamAsync();
            if (_selectedFile != null)
            {
                _memoryStream = new MemoryStream();
                await _selectedFile.CopyToAsync(_memoryStream);
                _selectedFile.Seek(0, SeekOrigin.Begin);
                SelectedImage = ImageSource.FromStream(() => _selectedFile);
                IsImageVisible = true;
                IsRemoveButtonVisible = true;               
            }
        }

        [RelayCommand]
        public void RemovePicture()
        {
            IsRemoveButtonVisible = false;
            SelectedImage = null;
            IsImageVisible = false;
            _selectedFile = null;

        }

        [RelayCommand]
        public async void CreatePost()
        {
            try
            {            
            PostDTO toPost = new PostDTO();
            string token = await _authenticationService.GetCurrentTokenAsync();

            toPost.PostingDate = DateTime.Now;
            toPost.Text = PostText;
            _manageData.SetStrategy(new CreateData());

            if (_selectedFile != null)
            {
                _memoryStream.Seek(0, SeekOrigin.Begin); // Reset the position of the memory stream to the beginning
                UpdateData updateData = new UpdateData();
                toPost.Image = await updateData.ManageStreamData($"Post/UploadPostImage", _memoryStream, token);
            }

            string json = JsonConvert.SerializeObject(toPost);
            _manageData.SetStrategy(new CreateData());
            await _manageData.GetDataAndDeserializeIt<string>($"Post/AddNewPostForUser", json, token);

            await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
