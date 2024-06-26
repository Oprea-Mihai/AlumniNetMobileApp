﻿using AlumniNetMobile.DTOs;
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
    public partial class SettingsView : ContentPage
    {
        public SettingsView(ProfileModel profile)
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(profile);
        }
    }
}