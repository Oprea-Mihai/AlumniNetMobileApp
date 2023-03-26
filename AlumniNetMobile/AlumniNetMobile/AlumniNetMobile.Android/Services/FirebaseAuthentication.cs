using AlumniNetMobile.Common;
using AlumniNetMobile.Droid.Services;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthentication))]
namespace AlumniNetMobile.Droid.Services
{
    public class FirebaseAuthentication : IAuthenticationService
    {
        public bool IsSignedIn()
        {
            return FirebaseAuth.Instance.CurrentUser != null;
        }
    }
}