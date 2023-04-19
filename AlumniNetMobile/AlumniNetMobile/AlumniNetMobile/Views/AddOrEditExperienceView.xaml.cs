using AlumniNetMobile.DTOs;
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
    public partial class AddOrEditExperienceView : ContentPage
    {
        public AddOrEditExperienceView()
        {
            InitializeComponent();
            BindingContext = new AddOrEditExperienceViewModel();
        }

        public AddOrEditExperienceView(ExperienceDTO workExperience)
        {
            InitializeComponent();
            BindingContext = new AddOrEditExperienceViewModel(workExperience);
        }


    }
}