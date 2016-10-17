using Tech_Tatva__16.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Tech_Tatva__16.Views;
using System.Xml.Serialization;
using Windows.Storage;
using Tech_Tatva__16.Classes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Tech_Tatva__16.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<int> Favs = new List<int>();

        public EventsPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
             Day day = new Day();
             day = e.NavigationParameter as Day;
            

            foreach(PivotItem p in MyPivot.Items)
            {
                if (p.Header.ToString().Equals(day.day))
                    MyPivot.SelectedItem = p;
            }

            DatabaseHelperClass db = new DatabaseHelperClass();
            List<EventClass> l = new List<EventClass>();
            l = db.ReadEvents();

            List<EventClass> Day1_Events = new List<EventClass>();
            List<EventClass> Day2_Events = new List<EventClass>();
            List<EventClass> Day3_Events = new List<EventClass>();
            List<EventClass> Day4_Events = new List<EventClass>();

            Day1_Events = l.Where(p => p.Day == "1").ToList();
            Day2_Events = l.Where(p => p.Day == "2").ToList();
            Day3_Events = l.Where(p => p.Day == "3").ToList();
            Day4_Events = l.Where(p => p.Day == "4").ToList();

            List<Day> list = new List<Day>();
            Day day1 = new Day();
            day1.Events = Day1_Events;
            day1.day = "day 1";

            Day day2 = new Day();
            day2.Events = Day2_Events;
            day2.day = "day 2";

            Day day3 = new Day();
            day3.Events = Day3_Events;
            day3.day = "day 3";

            Day day4 = new Day();
            day4.Events = Day4_Events;
            day4.day = "day 4";


            list.Add(day1);
            list.Add(day2);
            list.Add(day3);
            list.Add(day4);

            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values["Favs"] != null)
            {
                Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());
                if (Favs.Count != 0)
                {

                    foreach (int id in Favs)
                    {
                        foreach (Day days in list)
                        {
                            foreach (EventClass eve in days.Events)
                            {
                                if (id == eve.id)
                                {
                                    eve.Fav_Image = "ms-appx:///Assets/Icons/fav-icon_enabled.png";
                                }
                                else
                                {
                                    eve.Fav_Image = "ms-appx:///Assets/Icons/fav-icon_disabled.png";
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (EventClass eve in day.Events)
                    {
                        eve.Fav_Image = "ms-appx:///Assets/Icons/fav-icon_disabled.png";
                    }
                }
            }
            else
            {
                foreach(EventClass eve in day.Events)
                {
                    eve.Fav_Image = "ms-appx:///Assets/Icons/fav-icon_disabled.png";
                }
            }

            Day1.DataContext = Day1_Events;
            Day2.DataContext = Day2_Events;
            Day3.DataContext = Day3_Events;
            Day4.DataContext = Day4_Events;

        }

        public static string Serialize(object obj)
        {
            using (var sw = new StringWriter())
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            using (var sw = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sw);
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Event_Clicked(object sender, ItemClickEventArgs e)
        {
            
            Frame.Navigate(typeof(DetailsPage), e.ClickedItem as EventClass);
        }

        private void Favourites_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavouritesPage));
        }

        private void Dev_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DevelopersPage));
        }

        private void Abt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }
    }
}
