﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase;
using Plugin.CurrentActivity;

namespace AlumniNetMobile.Droid
{
    [Activity(Label = "AlumniNetMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                CrossCurrentActivity.Current.Init(this, savedInstanceState);
                Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

                FirebaseApp.InitializeApp(Application.Context);
                LoadApplication(new App());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}