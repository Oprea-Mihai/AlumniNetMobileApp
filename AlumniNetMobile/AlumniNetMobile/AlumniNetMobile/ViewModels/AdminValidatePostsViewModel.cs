using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AdminValidatePostsViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        #region Constructors
        public AdminValidatePostsViewModel()
        {

            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _postsWithoutImages = new List<PostModel>();
            Posts = new ObservableRangeCollection<PostModel>();
            _currentIndex = 0;
            _batchSize = 5;
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

        #region Methods

        private async Task<List<PostModel>> GetBatchOfPostsAsync(int batchSize, int index)
        {
            List<PostModel> postsBatch = new();
            try
            {
                string token = await _authenticationService.GetCurrentTokenAsync();
                _manageData.SetStrategy(new GetData());
                postsBatch = await _manageData.GetDataAndDeserializeIt
                    <List<PostModel>>
                    ($"Post/GetBatchOfPostsForAdmin?batchSize={batchSize}&currentIndex={index}", "", token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return postsBatch;
        }

        private async Task ValidatePostAsync(int postId, bool isValid)
        {
            _manageData.SetStrategy(new UpdateData());
            string token = await _authenticationService.GetCurrentTokenAsync();
            await _manageData.GetDataAndDeserializeIt<bool>
                ($"Post/ValidatePostById?postId={postId}&isValid={isValid}", "", token);
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
            Posts = new ObservableRangeCollection<PostModel>();
            Posts.AddRange(postsWithImgs);
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
        private bool _isBusy;

        [ObservableProperty]
        private bool _noPostsAvailableVisible;
        #endregion

        #region Commands

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
        public async void AcceptPostAsync(int postId)
        {
            if (IsBusy) return;

            await ValidatePostAsync(postId, true);

            Posts.Remove(Posts.Where(p => p.PostId == postId).First());
            if (Posts.Count == 0)
            {
                NoPostsAvailableVisible = true;
            }

            IsBusy = false;
        }

        [RelayCommand]
        public async void RejectPostAsync(int postId)
        {
            if (IsBusy) return;

            await ValidatePostAsync(postId, false);

            Posts.Remove(Posts.Where(p => p.PostId == postId).First());

            if (Posts.Count == 0)
            {
                NoPostsAvailableVisible = true;
            }

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

            if (Posts.Count == 0)
            {
                NoPostsAvailableVisible = true;
            }
        }

        [RelayCommand]
        public async void PageAppearing()
        {
            await InitializeAsync();
        }

        #endregion
    }
}
