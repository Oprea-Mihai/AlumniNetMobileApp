using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class HomeViewModel:ObservableObject
    {

        #region Constructors
        public HomeViewModel()
        {
            
        }
        #endregion

        #region Private fields
        #endregion

        #region Observables
        #endregion

        #region Commands
        [RelayCommand]
        public void SeachButtonClicked()
        {
            Application.Current.MainPage.Navigation.PushAsync(new SearchView());
        }
        #endregion

    }
}
