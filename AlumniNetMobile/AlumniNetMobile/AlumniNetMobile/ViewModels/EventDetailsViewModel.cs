using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class EventDetailsViewModel : ObservableObject
    {
        #region Constructors
        public EventDetailsViewModel()
        {
            
        }

        public EventDetailsViewModel(EventModel selectedEvent)
        {
            _selectedEvent = selectedEvent;
            Title = selectedEvent.EventName;
            ImageSrc = selectedEvent.ImageSource;
            StartDate= selectedEvent.StartDate;
            Description = selectedEvent.Description;
        }
     
        #endregion

        #region Private fields

        private EventModel _selectedEvent;

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
        public void Accept()
        {

        }

        [RelayCommand]
        public void Reject()
        {

        }

        #endregion
    }
}
