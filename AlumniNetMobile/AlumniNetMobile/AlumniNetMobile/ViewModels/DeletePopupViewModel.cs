using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class DeletePopupViewModel : Popup
    {
        #region PrivateFields...
        private readonly ManageData _manageData;
        private int _idToDelete;
        private IAuthenticationService _authenticationService;
        #endregion

        #region Constructors...
        public DeletePopupViewModel(int idToDelete)
        {
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _manageData = new ManageData();
            _idToDelete = idToDelete;
        }
        #endregion

        #region Commands...

        [RelayCommand]
        private async Task Delete()
        {
            try
            {
                string token = await _authenticationService.GetCurrentTokenAsync();

                _manageData.SetStrategy(new DeleteData());
                await _manageData.GetDataAndDeserializeIt<bool>
                    ($"FinishedStudy/DeleteFinishedStudy?id={_idToDelete}", "", token);

                await Application.Current.MainPage.Navigation.PopAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        #endregion
    }
}
