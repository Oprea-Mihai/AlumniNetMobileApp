using AlumniNetMobile.Models;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class HomeViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        #region Constructors
        public HomeViewModel()
        {
            Posts = new ObservableRangeCollection<PostModel>
                {
                    new PostModel { Username = "John Doe", Text = "Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.", Title = "My First Post" },
                    new PostModel { Username = "Jane Doe", Text = "Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.", Title = "Another Post" },
                    new PostModel { Username = "Bob Smith", Text = "Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.Lorem ipsum dolor sit amet.", Title = "Third Post" }
                };
            IsLikeButtonClicked = true;
        }
        #endregion

        #region Private fields
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

        #endregion

    }
}
