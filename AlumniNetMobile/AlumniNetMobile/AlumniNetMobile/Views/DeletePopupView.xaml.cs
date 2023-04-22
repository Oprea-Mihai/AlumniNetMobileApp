using AlumniNetMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumniNetMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletePopupView : Popup
    {
        #region Constructors...

        public DeletePopupView(int idToDelete)
        {
            InitializeComponent();
            BindingContext = new DeletePopupViewModel(idToDelete);
        }

        #endregion

        #region Methods...

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Dismiss(null);
        }
        void YesDeleteButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Dismiss(null);
        }

        #endregion
    }
}