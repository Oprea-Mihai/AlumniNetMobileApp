using AlumniNetMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumniNetMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminSettingsView : ContentPage
    {
        public AdminSettingsView()
        {
            InitializeComponent();
            BindingContext = new AdminSettingsViewModel();
        }
    }
}