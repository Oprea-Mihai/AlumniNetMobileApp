using AlumniNetMobile.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AlumniNetMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteSpecializationPopupView : Popup
    {
        #region Constructors...

        public DeleteSpecializationPopupView(int idToDelete)
        {
            InitializeComponent();
            BindingContext = new DeleteSpecializaitonPopupViewModel(idToDelete);
        }

        #endregion

        #region Methods...

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Dismiss(null);
        }
      

        #endregion
    }
}