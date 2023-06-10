using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AdminManageEventsViewModel : ObservableObject
    {
        #region Constructors
        public AdminManageEventsViewModel()
        {

        }
        #endregion

        #region Private fields
        #endregion

        #region Methods
        #endregion

        #region Observables
        #endregion

        #region Commands

        [RelayCommand]
        public async void AddEvent()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AdminAddEventView());
        }

        #endregion

    }
}
