using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class EventDetailsViewModel : ObservableObject
    {
        #region Constructors
        public EventDetailsViewModel()
        {

        }

        public EventDetailsViewModel(EventInviteModel selectedEvent)
        {
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _manageData = new ManageData();

            _selectedEvent = selectedEvent;

            Title = selectedEvent.EventName;
            StartDate = selectedEvent.StartDateString;
            Description = selectedEvent.Description;
        }


        #endregion

        #region Private fields

        private IAuthenticationService _authenticationService;
        private EventInviteModel _selectedEvent;
        private readonly ManageData _manageData;

        #endregion

        #region Methods
        #endregion

        #region Observables

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private ImageSource _imageSrc;

        [ObservableProperty]
        private string _startDate;

        #endregion

        #region Commands

        [RelayCommand]
        public async void Accept()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new UpdateData());
           await _manageData.GetDataAndDeserializeIt<string>
                ($"InvitedUser/AnswerEventInvite?inviteId={_selectedEvent.InviteId}&answer={true}","",token);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        public async void Reject()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new UpdateData());
            await _manageData.GetDataAndDeserializeIt<string>
                 ($"InvitedUser/AnswerEventInvite?inviteId={_selectedEvent.InviteId}&answer={false}", "", token);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        public async void PageAppearing()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            string picKey = _selectedEvent.Image;
            if (picKey != null && picKey != "")
            {
                GetData getData = new GetData();
                Stream file = await getData.ManageStreamData($"Files/GetFileByKey?key={picKey}", token);
                ImageSrc = ImageSource.FromStream(() => file);
            }
        }
        #endregion
    }
}
