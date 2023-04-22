using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Models;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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

            Posts = new ObservableRangeCollection<PostModel>();
            IsLikeButtonClicked = true;

            _currentIndex = 0;
            _batchSize = 5;
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();

        }
        #endregion

        #region Private fields

        private ManageData _manageData;
        public int _currentIndex;
        private readonly int _batchSize;
        private IAuthenticationService _authenticationService;

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
                    ($"Post/GetBatchOfPostsSorted?batchSize={batchSize}&currentIndex={index}", "",token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return postsBatch;
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
        public void LikeButtonClicked(PostModel obj)
        {
            IsLikeButtonClicked = !IsLikeButtonClicked;
        }

        [RelayCommand]
        public async void LoadMorePostsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            Posts.AddRange(await GetBatchOfPostsAsync(_batchSize, _currentIndex));

            _currentIndex++;
            IsBusy = false;
        }


        [RelayCommand]
        public async Task InitializeAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            _currentIndex = 1;

            Posts.ReplaceRange(await GetBatchOfPostsAsync(_batchSize, 0));

            IsBusy = false;
        }

        [RelayCommand]
        public async void PageAppearing()
        {
            await InitializeAsync();
        }
        #endregion

    }
}
