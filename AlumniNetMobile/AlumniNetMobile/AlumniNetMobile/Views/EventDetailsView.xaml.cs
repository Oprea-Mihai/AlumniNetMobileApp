using AlumniNetMobile.Models;
using AlumniNetMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumniNetMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailsView : ContentPage
    {
        public EventDetailsView()
        {
            InitializeComponent();
            BindingContext = new EventDetailsViewModel();
        }

        public EventDetailsView(EventInviteModel selectedEvent)
        {
            InitializeComponent();
            BindingContext = new EventDetailsViewModel(selectedEvent);
        }

    }
}