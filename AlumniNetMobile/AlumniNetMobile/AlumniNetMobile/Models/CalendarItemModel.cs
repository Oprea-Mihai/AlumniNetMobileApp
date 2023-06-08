using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.Models
{
    public partial class CalendarItemModel : ObservableObject
    {
        #region Constructors...

        public CalendarItemModel(DateTime date)
        {
            Date = date.Date;
            DisplayType = "date";
            Label = Date.Day.ToString();
        }
        public CalendarItemModel(string label = " ", string type = "empty")
        {
            DisplayType = type;
            Label = label;
        }

        #endregion

        #region Private fields...
        
        [ObservableProperty]
        private string _displayType;

      
        #endregion

        #region Public fields...

        public DateTime Date { get; set; }
        public string Label { get; }

        #endregion
    }
}
