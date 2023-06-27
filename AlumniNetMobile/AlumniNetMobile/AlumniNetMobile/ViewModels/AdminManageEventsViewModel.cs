using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFImageLoading.Concurrency;
using System.Collections.Generic;
using System.IO;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AdminManageEventsViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors
        public AdminManageEventsViewModel()
        {
            IsBusy = false;

            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();

            Events=new ObservableRangeCollection<EventModel> ();
        }
        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Methods
        #endregion

        #region Observables

        [ObservableProperty]
        private EventModel _selectedEvent;

        [ObservableProperty]
        private bool _isBusy;

        ObservableRangeCollection<EventModel> _events;
        public ObservableRangeCollection<EventModel> Events
        {
            get { return _events; }
            set { SetProperty(ref _events, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        public async void AddEvent()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AdminAddEventView());
        }

        [RelayCommand]
        public async void OpenSelectedEvent()
        {
            if (SelectedEvent == null)
                return;
            var ev = SelectedEvent;
            SelectedEvent = null;
            await Application.Current.MainPage.Navigation.PushAsync(new AdminAddEventView(ev));
        }



        [RelayCommand]
        public async void PageAppearing()
        {
            if (IsBusy) return;
            IsBusy = true;

            _manageData.SetStrategy(new GetData());
            string token = await _authenticationService.GetCurrentTokenAsync();
            List<EventModel> events = await _manageData.
                GetDataAndDeserializeIt<List<EventModel>>("Event/GetAllEventsWithInviteResults", "", token);

            foreach (EventModel eventModel in events)
            {
                var picKey = eventModel.Image;
                if (picKey != null && picKey != "")
                {
                    GetData getData = new GetData();
                    Stream file = await getData.ManageStreamData($"Files/GetFileByKey?key={picKey}", token);
                    eventModel.ImageSource = ImageSource.FromStream(() => file);
                }
            }
            Events.ReplaceRange(events);
        
            IsBusy = false;
        }



        #endregion

    }
}
