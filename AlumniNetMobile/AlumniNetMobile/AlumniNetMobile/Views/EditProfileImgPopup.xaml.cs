using AlumniNetMobile.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;


namespace AlumniNetMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfileImgPopup : Popup
    {
        public EditProfileImgPopup(string? imageKey)
        {
            InitializeComponent();
            BindingContext = new EditProfileImgViewModel(imageKey);
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Dismiss(null);
        }
    }
}