using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class DeleteExperiencePopupViewModel:ObservableObject
    {
        #region Constructors

        public DeleteExperiencePopupViewModel(int idToDelete)
        {
            _idToDelete = idToDelete;
            _manageData=new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
        }

        #endregion

        #region Private fields

        int _idToDelete;
        private ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Commands

        [RelayCommand]
        public async void Delete()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();

            _manageData.SetStrategy(new DeleteData());
            await _manageData.GetDataAndDeserializeIt<bool>($"Experience/DeleteExperience?experienceId={_idToDelete}", "", token);


            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
