using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using AlumniNetMobile.Resx;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace AlumniNetMobile.ViewModels
{
    public partial class AdminAddEventViewModel : ObservableObject
    {
        #region Constructors
        public AdminAddEventViewModel()
        {
            IsCalendarVisible = false;
            CalendarItems = new ObservableRangeCollection<CalendarItemModel>();
            CalendarMonth = DateTime.Now;
            StartDateButtonText = GetLocalizedString("SelectStartDate");
            SearchedFacultyName = "";
            _areFacultySugestionsVisible = false;
            wasFacultyTextChanged = false;
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _manageData = new ManageData();

            AvailablePromotionYears = new ObservableRangeCollection<int>();
            DisplayedFacultyNames = new ObservableRangeCollection<FacultyModel>();
            SelectedFacultyNames = new ObservableRangeCollection<FacultyModel>();
        }
        #endregion

        #region Private fields     
        private bool wasFacultyTextChanged;
        private ManageData _manageData;
        private IAuthenticationService _authenticationService;
        #endregion

        #region Calendar logic  

        private CalendarItemModel _calendarSelectedItem;
        private CalendarItemModel _calendarPreviousSelectedItem;
        private DateTime _calendarMonth;
        public ObservableRangeCollection<CalendarItemModel> CalendarItems { get; set; }
        public string CalendarMonthText => _calendarMonth.ToString("MMMM yyyy");

        public DateTime CalendarMonth
        {
            get => _calendarMonth;
            set
            {
                SetProperty(ref _calendarMonth, value);
                //As the month has been changed you need to tell the UI that the CalendarMonthText property has also changed
                OnPropertyChanged(nameof(CalendarMonthText));
                //Lets create all the items month selected
                CalendarCreateItemsForMonth(CalendarMonth);
            }
        }

        public CalendarItemModel CalendarSelectedItem
        {
            get => _calendarSelectedItem;
            set
            {
                //if the selects the same item just return (nothing to be done)
                if (_calendarSelectedItem == value) return;

                //if the item has the type or empty or header lets not actually do anything
                if (value.DisplayType == "empty" || value.DisplayType == "unavailable" || value.DisplayType == "header") return;

                //If we get here it must be a date so set the selected date
                CalendarSelectedDate = value;
                StartDateButtonText = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(CalendarSelectedDate.Date.Month).ToString() +
                                            " " + CalendarSelectedDate.Date.Day.ToString() +
                                            ", " + CalendarSelectedDate.Date.Year.ToString();

                if (_calendarSelectedItem != null)
                {
                    _calendarPreviousSelectedItem = _calendarSelectedItem;
                }

                SetProperty(ref _calendarSelectedItem, value);
            }
        }

        private void CalendarCreateItemsForMonth(DateTime date)
        {
            //lets start by creating the header row
            var items = new List<CalendarItemModel>();


            //default to Calendar of the month for the date past in
            var dateToCalc = new DateTime(date.Year, date.Month, 1);
            var lastMonth = new DateTime(dateToCalc.AddMonths(-1).Year, dateToCalc.AddMonths(-1).Month, 1);
            var lastMonthDays = DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
            //This is done to add blank items for when the month does not start on a Sunday to the items align with the header
            if (dateToCalc.DayOfWeek != DayOfWeek.Sunday)
            {
                for (var i = 0; i < (int)dateToCalc.DayOfWeek; i++)
                {
                    int lastMonthDateNumber = lastMonthDays - (int)dateToCalc.DayOfWeek + i + 1;
                    var lastMonthDate = new DateTime(lastMonth.Year, lastMonth.Month, lastMonthDateNumber);

                    var lastMonthDay = new CalendarItemModel(lastMonthDate);
                    lastMonthDay.DisplayType = "unavailable";
                    items.Add(lastMonthDay);
                }
            }

            //This loop adds an item for every day of the current month selected
            var currentMonth = date.Month;
            while (dateToCalc.Month == currentMonth)
            {
                var day = new CalendarItemModel(dateToCalc);
                //if the date for the item it is creating then flag it as today so it can be styled differently
                if (day.Date.Date == DateTime.Now.Date) day.DisplayType = "today";

                items.Add(day);
                dateToCalc = dateToCalc.AddDays(1);
            }

            var nextMonth = dateToCalc.AddMonths(1);
            while (items.Count < 42)
            {
                var nextMonthDay = new CalendarItemModel(nextMonth);
                nextMonthDay.DisplayType = "unavailable";
                items.Add(nextMonthDay);
                nextMonth = nextMonth.AddDays(1);
            }


            //Replace Range is used so only one call to update the UI is called
            CalendarItems.ReplaceRange(items);
        }


        //On changing the month lets check if the current month contains the selected date so it can be set up correctly
        private void CalendarCheckSelected()
        {
            if (CalendarSelectedDate != null)
            {
                var d = CalendarItems.FirstOrDefault(x => x.Date == CalendarSelectedDate.Date);
                if (d != null)
                {
                    CalendarSelectedItem = d;
                }
            }
        }


        [RelayCommand]
        public void CalendarPreviousMonth()
        {
            CalendarMonth = CalendarMonth.AddMonths(-1); CalendarCheckSelected();
        }

        [RelayCommand]
        public void CalendarNextMonth()
        {
            CalendarMonth = CalendarMonth.AddMonths(1); CalendarCheckSelected();
        }


        [RelayCommand]
        public void CalendarDateChanged()
        {
            if (_calendarPreviousSelectedItem != null)
            {
                var item = CalendarItems.FirstOrDefault(x => x == _calendarPreviousSelectedItem);
                if (item != null)
                {
                    item.DisplayType = DateTime.Now.Date == _calendarPreviousSelectedItem.Date.Date ? "today" : "date";
                }
            }

            var selectedItem = CalendarItems.FirstOrDefault(x => x == CalendarSelectedItem);
            if (selectedItem is not null)
            {
                selectedItem.DisplayType = "selected";
                IsCalendarVisible = false;
                StartDate = CalendarSelectedDate.Date;
            }

        }
        #endregion

        #region Methods...

        private static string GetLocalizedString(string key)
        {
            ResourceManager rm = new ResourceManager("AlumniNetMobile.Resx.AppResource", typeof(AppResource).Assembly);
            return rm.GetString(key, CultureInfo.CurrentCulture);
        }



        #endregion

        #region Observables

        [ObservableProperty]
        private CalendarItemModel _calendarSelectedDate;

        [ObservableProperty]
        private string _startDateButtonText;

        [ObservableProperty]
        private DateTime _startDate;

        [ObservableProperty]
        private bool _isCalendarVisible;

        [ObservableProperty]
        private bool _yearChecked;

        [ObservableProperty]
        private bool _nameChecked;

        [ObservableProperty]
        private bool _everyoneChecked;

        [ObservableProperty]
        private bool _facultyChecked;

        [ObservableProperty]
        private string _searchedFacultyName;

        [ObservableProperty]
        private FacultyModel _selectedFaculty;

        [ObservableProperty]
        private bool _areFacultySugestionsVisible;

        [ObservableProperty]
        private bool _facultyNotFoundVisible;

        public ObservableRangeCollection<FacultyModel> _displayedFacultyNames;

        public ObservableRangeCollection<FacultyModel> DisplayedFacultyNames
        {
            get { return _displayedFacultyNames; }
            set { SetProperty(ref _displayedFacultyNames, value); }
        }

        public ObservableRangeCollection<FacultyModel> _selectedFacultyNames;

        public ObservableRangeCollection<FacultyModel> SelectedFacultyNames
        {
            get { return _selectedFacultyNames; }
            set { SetProperty(ref _selectedFacultyNames, value); }
        }

        public ObservableRangeCollection<int> _availablePromotionYears;

        public ObservableRangeCollection<int> AvailablePromotionYears
        {
            get { return _availablePromotionYears; }
            set { SetProperty(ref _availablePromotionYears, value); }
        }

        public ObservableRangeCollection<int> _selectedPromotionYears;

        public ObservableRangeCollection<int> SelectedPromotionYears
        {
            get { return _selectedPromotionYears; }
            set { SetProperty(ref _selectedPromotionYears, value); }
        }

        #endregion

        #region Commands

        [RelayCommand]
        public void StartDateButtonClicked()//to show first calendar
        {
            IsCalendarVisible = !IsCalendarVisible;
        }

        [RelayCommand]
        public void EveryoneCheckedChanged()
        {
            if (EveryoneChecked == true)
                FacultyChecked = NameChecked = YearChecked = false;
        }

        [RelayCommand]
        public async Task YearCheckedChangedAsync()
        {
            if (YearChecked == true)
            {
                //string token = await _authenticationService.GetCurrentTokenAsync();
                _manageData.SetStrategy(new GetData());
                List<int> years = await _manageData.GetDataAndDeserializeIt<List<int>>
                    ("FinishedStudy/GetAllFinishingYears", "", "");

                AvailablePromotionYears.ReplaceRange(years);
                EveryoneChecked = NameChecked = FacultyChecked = false;
            }
        }

        [RelayCommand]
        public void NameCheckedChanged()
        {
            if (NameChecked == true)
                EveryoneChecked = YearChecked = FacultyChecked = false;
        }

        [RelayCommand]
        public void FacultyCheckedChanged()
        {
            if (FacultyChecked == true)
            {
                EveryoneChecked = NameChecked = YearChecked = false;
               
            }

        }

        [RelayCommand]
        public void FacultyTextChanged()
        {
            wasFacultyTextChanged = true;
        }

        [RelayCommand]
        public async void SearchFaculty()
        {
            FacultyNotFoundVisible = false;
            if (wasFacultyTextChanged == true)
            {
                SelectedFaculty = null;
            }

            if (wasFacultyTextChanged)
            {
                //to delete
                DisplayedFacultyNames.ReplaceRange(new List<FacultyModel>
                { new FacultyModel {FacultyId=1, FacultyName="Test1" },
                new FacultyModel {FacultyId=2, FacultyName="Test2" }});

                //to keep

                DisplayedFacultyNames.RemoveRange(DisplayedFacultyNames.
                    Where(x=>SelectedFacultyNames.Any(y=>y.FacultyName==x.FacultyName)).ToList());
                
                AreFacultySugestionsVisible = true;

                //!!!UNCOMMENT WHEN INTERNET CONNECTION RESORED|DELETE THE ABOVE
                //string token = await _authenticationService.GetCurrentTokenAsync();
                //_manageData.SetStrategy(new GetData());
                //List<FacultyModel> names = (await _manageData.GetDataAndDeserializeIt<List<FacultyModel>>
                //    ($"Faculty/GetFacultiesSearchSuggestions?searchedString={SearchedFacultyName}", "", token));
                //if (names.Count() != 0)
                //{
                //    DisplayedFacultyNames = new ObservableRangeCollection<FacultyModel>(names);
                //    AreFacultySugestionsVisible = true;
                //}
                //else
                //{
                //    AreFacultySugestionsVisible = false;
                //    FacultyNotFoundVisible = true;
                //}
            }
        }

        [RelayCommand]
        public async void FacultySelected()
        {
            if (SelectedFaculty == null)
                return;
            SearchedFacultyName = SelectedFaculty.FacultyName;
            if (!SelectedFacultyNames.Contains(SelectedFaculty))
            SelectedFacultyNames.Add(SelectedFaculty);
            DisplayedFacultyNames.Remove(SelectedFaculty);
            SelectedFaculty = null;
            //AreFacultySugestionsVisible = false;
            wasFacultyTextChanged = false;
        }
        #endregion

    }
}
