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
	public partial class AuthenticationView : ContentPage
	{
		public AuthenticationView ()
		{
			InitializeComponent ();
			BindingContext = new AuthenticationViewModel();
		}
	}
}