using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class EventsViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors
        public EventsViewModel()
        {
            Events = new ObservableRangeCollection<EventModel>();
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
        }
        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Methods



        #endregion

        #region Observables

        ObservableRangeCollection<EventModel> _events;
        public ObservableRangeCollection<EventModel> Events
        {
            get { return _events; }
            set { SetProperty(ref _events, value); }
        }

        [ObservableProperty]
        private bool _isBusy = false;

        [ObservableProperty]
        private EventModel _selectedEvent;

        #endregion

        #region Commands

        [RelayCommand]
        public async void OpenSelectedEvent()
        {
            if (SelectedEvent == null)
                return;
            var ev = SelectedEvent;
            SelectedEvent = null;
            await Application.Current.MainPage.Navigation.PushAsync(new EventDetailsView(ev));
        }

        [RelayCommand]
        public async void PageAppearing()
        {
            _manageData.SetStrategy(new GetData());
            string token=await _authenticationService.GetCurrentTokenAsync();
            List<EventModel> events = await _manageData.
                GetDataAndDeserializeIt<List<EventModel>>("Event/GetEventsForUser", "", token);

            foreach (EventModel eventModel in events)
            {
                var picKey = eventModel.Image;
                if (picKey != null && picKey != "")
                {
                   // GetData getData = new GetData();
                   // Stream file = await getData.ManageStreamData($"Files/GetFileByKey?key={picKey}", token);
                    eventModel.ImageSource = ImageSource.FromStream(() => file);
                }
            }
                Events.ReplaceRange(events);
        }

        #endregion
    }
}
