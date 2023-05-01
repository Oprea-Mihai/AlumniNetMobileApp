using AlumniNetMobile.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AlumniNetMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteExperiencePopup : Popup
    {
        public DeleteExperiencePopup(int idToDelete)
        {
            InitializeComponent();
            BindingContext = new DeleteExperiencePopupViewModel(idToDelete);
        }

        #region Methods...

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Dismiss(null);
        }

        #endregion
    }
}