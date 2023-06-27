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
    public partial class AdminNavigation : TabbedPage
    {
        public AdminNavigation ()
        {
            InitializeComponent();
            this.CurrentPage = this.Children[1];
        }
    }
}