using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Models;
using AlumniNetMobile.Resx;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class HomeViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        #region Constructors
        public HomeViewModel()
        {
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _postsWithoutImages = new List<PostModel>();
            Posts = new ObservableRangeCollection<PostModel>();
            _correctNavigation = false;
            _currentIndex = 0;
            _batchSize = 5;

            IsLikeButtonClicked = true;
        }
        #endregion

        #region Private fields

        private bool _correctNavigation;
        private IAuthenticationService _authenticationService;
        private List<PostModel> _postsWithoutImages;
        private ManageData _manageData;
        public int _currentIndex;
        private readonly int _batchSize;

        #endregion

        #region Private methods

        private async Task<List<PostModel>> GetBatchOfPostsAsync(int batchSize, int index)
        {
            List<PostModel> postsBatch = new();
            try
            {
                string token = await _authenticationService.GetCurrentTokenAsync();
                _manageData.SetStrategy(new GetData());
                postsBatch = await _manageData.GetDataAndDeserializeIt
                    <List<PostModel>>
                    ($"Post/GetBatchOfPostsSorted?batchSize={batchSize}&currentIndex={index}", "", token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return postsBatch;
        }

        private async Task LoadPostsWithImages()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            List<PostModel> postsWithImgs = new List<PostModel>();
            foreach (var post in _postsWithoutImages)
            {
                var picKey = post.Image;
                if (picKey != null && picKey != "")
                {
                    GetData getData = new GetData();
                    Stream file = await getData.ManageStreamData($"Files/GetFileByKey?key={picKey}", token);
                    post.ImageSource = ImageSource.FromStream(() => file);
                    postsWithImgs.Add(post);
                }
            }
           // Posts = new ObservableRangeCollection<PostModel>();
            Posts.AddRange(postsWithImgs);
        }

        private async void SetCultureForCurrentUser()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new GetData());
            string userLanguage = await _manageData.GetDataAndDeserializeIt<string>($"User/GetUserLanguage", "", token);
            CultureInfo language = new CultureInfo(userLanguage);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(userLanguage);
            AppResource.Culture = language;
        }

        private async Task DisplayCorrectNavigationForUser()
        {
            if (_correctNavigation == true)
                return;
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new GetData());
            UserDTO user = await _manageData.GetDataAndDeserializeIt<UserDTO>("User/GetUserById", "", token);
            if (!user.IsValid)
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new ProfileView();
                });
            else if (user.IsAdmin == true)
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new AdminNavigation());
                });
            _correctNavigation = true;

        }
        #endregion

        #region Observables

        private ObservableRangeCollection<PostModel> _posts;

        public ObservableRangeCollection<PostModel> Posts
        {
            get { return _posts; }
            set { SetProperty(ref _posts, value); }
        }

        [ObservableProperty]
        private bool _isLikeButtonClicked;

        [ObservableProperty]
        private bool _isSaveButtonClicked;

        [ObservableProperty]
        private bool _isBusy;

        #endregion

        #region Commands

        [RelayCommand]
        public void SearchButtonClicked()
        {
            Application.Current.MainPage.Navigation.PushAsync(new SearchView());
        }

        [RelayCommand]
        public async Task AddPostButtonClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPostView());
        }

        [RelayCommand]
        public void LikeButtonClicked(PostModel obj)
        {
            IsLikeButtonClicked = !IsLikeButtonClicked;
        }


        [RelayCommand]
        public async void LoadMorePostsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            _postsWithoutImages.AddRange(await GetBatchOfPostsAsync(_batchSize, _currentIndex));
            await LoadPostsWithImages();

            _currentIndex++;
            IsBusy = false;
        }


        [RelayCommand]
        public async Task InitializeAsync()
        {

            _postsWithoutImages = new List<PostModel>();
            Posts = new ObservableRangeCollection<PostModel>();

            if (IsBusy) return;

            IsBusy = true;

            _currentIndex = 1;
            _postsWithoutImages.AddRange(await GetBatchOfPostsAsync(10, 0));
            await LoadPostsWithImages();
            IsBusy = false;
        }

        [RelayCommand]
        public async void PageAppearing()
        {
            SetCultureForCurrentUser();
            await DisplayCorrectNavigationForUser();
            await InitializeAsync();
        }
        #endregion

    }
}
