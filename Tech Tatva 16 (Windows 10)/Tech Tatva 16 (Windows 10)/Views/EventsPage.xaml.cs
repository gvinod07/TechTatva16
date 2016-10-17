using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tech_Tatva_16__Windows_10_.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Tech_Tatva_16__Windows_10_.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsPage : Page
    {


        public ObservableCollection<Day> Days = new ObservableCollection<Day>();
        public ObservableCollection<Day> Favourites = new ObservableCollection<Day>();
        public List<int> Favs = new List<int>();
        private bool handle = true;

        public static EventsPage Instance;

        bool RefInBack = false;

        public EventsPage()
        {

            this.InitializeComponent();

            Instance = this;

            Filter_Fav.SelectedIndex = 0;

            Day dayfav = new Day();
            dayfav.day = "";

            ObservableCollection<EventClass> lis = new ObservableCollection<EventClass>();

            foreach (Day d in Days)
            {
                foreach (EventClass events in d.Events)
                {
                    if (events.Fav_Image.Equals(""))
                    {
                        lis.Add(events);
                    }
                }
            }

            if (lis.Count == 0)
                dayfav.day = "No Favourites Added Yet..";

            dayfav.Events = lis;
            Favourites.Add(dayfav);

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values["Favs"] != null)
            {
                Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());
            }

            DatabaseHelperClass db = new DatabaseHelperClass();
            string myPages = "";
            foreach (PageStackEntry page in Frame.BackStack)
            {
                myPages += page.SourcePageType.ToString() + "\n";
            }

            if (Frame.CanGoBack && Frame.BackStackDepth > 1)
            {

                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            if ((string)e.Parameter != "" && e.Parameter != null && e.Parameter.GetType() == typeof(string))
            {
                ObservableCollection<EventClass> l = new ObservableCollection<EventClass>();
                EventClass eve = db.ReadEventByName((string)e.Parameter);
                if (eve == null)
                {
                    l.Add(eve);
                    Days.Clear();
                    Day day = new Day();
                    day.Events = l;
                    day.day = "Event Not Found";
                    Days.Add(day);
                }
                else
                {
                    l.Add(eve);
                    Days.Clear();
                    Day day = new Day();
                    day.Events = l;
                    day.day = "Event Found";
                    Days.Add(day);
                }
            }
            else
            {

                List<EventClass> list = new List<EventClass>();

                //Start of API Calls
                if (IsInternet())
                {
                    if ((db.ReadEvents() as List<EventClass>).Count == 0)
                    {
                        List<EventClass> listevents = new List<EventClass>();
                        listevents = await GetEventsAPIAsync();
                        db.Insert(listevents);
                    }
                    else
                        RefInBack = true;
                }
                else
                {
                    if ((db.ReadEvents() as List<EventClass>).Count == 0)
                        MainPage.Instance.ShowPopup();
                    else
                        MainPage.Instance.HidePopup();
                }

                AssignItemSource();
            }
        }

        private void Event_Clicked(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                ListViewItem _Item = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                var _Children = AllChildren(_Item);
                var _Name = "DetailsPanel";
                var _Control = (StackPanel)_Children.First(c => c.Name == _Name);

                _Control.Visibility = Visibility.Visible;

                // you will get slected item here. Use that item to get listbox item
            }
            foreach (var item in e.RemovedItems)
            {
                ListViewItem _Item = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                var _Children = AllChildren(_Item);
                var _Name = "DetailsPanel";
                var _Control = (StackPanel)_Children.First(c => c.Name == _Name);

                _Control.Visibility = Visibility.Collapsed;
            }
        }

        public List<StackPanel> AllChildren(DependencyObject parent)
        {
            var _List = new List<StackPanel> { };
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is StackPanel)
                    _List.Add(_Child as StackPanel);

                _List.AddRange(AllChildren(_Child));
            }

            return _List;
        }

        private void Fav_Button_Click(object sender, RoutedEventArgs e)
        {
            List<int> Favs = new List<int>();
            var roamingSettings = ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values["Favs"] != null)
            {
                Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());
            }

            DatabaseHelperClass db = new DatabaseHelperClass();
            EventClass eve = (sender as RadioButton).DataContext as EventClass;

            if ((sender as RadioButton).Tag.Equals(""))
            {
                (sender as RadioButton).Tag = ("");
                (sender as RadioButton).Content = "Remove Bookmark";

                if (Favs.Contains(eve.id))
                {
                    //Do Nothing
                }
                else
                {
                    Favs.Add(eve.id);
                }
            }

            else if ((sender as RadioButton).Tag.Equals(""))
            {
                (sender as RadioButton).Tag = ("");
                (sender as RadioButton).Content = "Bookmark Event";

                Favs.Remove(eve.id);
            }


            roamingSettings.Values["Favs"] = Serialize(Favs);

            RefreshLayout();

        }

        public List<Control> AllChildrenControls(DependencyObject parent)
        {
            var _List = new List<Control> { };
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);

                _List.AddRange(AllChildrenControls(_Child));
            }

            return _List;
        }

        public void Filter_Fav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<int> Favs = new List<int>();
            if ((sender as ComboBox).SelectedIndex == 1)
            {
                Day dayfav = new Day();
                dayfav.day = "";

                ObservableCollection<EventClass> FavEvents = new ObservableCollection<EventClass>();


                var roamingSettings = ApplicationData.Current.RoamingSettings;
                if (roamingSettings.Values["Favs"] != null)
                {
                    Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());
                }

                if (Favs.Count == 0)
                    dayfav.day = "No Favourites Added Yet..";

                else
                {
                    DatabaseHelperClass db = new DatabaseHelperClass();

                    foreach (int id in Favs)
                    {
                        if (db.ReadEventById(id) != null)
                        {
                            FavEvents.Add(db.ReadEventById(id));
                        }
                    }

                    foreach (EventClass eve in FavEvents)
                    {
                        eve.Fav_Image = "";
                    }

                }
                dayfav.Events = FavEvents;

                Favourites.Clear();
                Favourites.Add(dayfav);
                MyPivot.ItemsSource = Favourites;

            }
            else
            {
                AssignItemSource();
                MyPivot.ItemsSource = Days;

            }
        }

        private void Fav_Button_Loaded(object sender, RoutedEventArgs e)
        {
            EventClass eve = (sender as RadioButton).DataContext as EventClass;

            if (eve != null)
            {
                if ((sender as RadioButton).Tag.Equals(""))
                {
                    (sender as RadioButton).Content = "Bookmark Event";
                }

                else if ((sender as RadioButton).Tag.Equals(""))
                {
                    (sender as RadioButton).Content = "Remove Bookmark";
                }
            }
        }

        private void Filter_Button_Clicked(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        private async Task<List<EventClass>> GetEventsAPIAsync()
        {
            List<EventClass> eve = new List<EventClass>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    ListEventAPI E1 = new ListEventAPI();
                    ListSchedule E2 = new ListSchedule();
                    var response = await client.GetStringAsync("http://api.mitportals.in/events");
                    E1 = JsonConvert.DeserializeObject<ListEventAPI>(response);

                    var response1 = await client.GetStringAsync("http://api.mitportals.in/schedule");
                    E2 = JsonConvert.DeserializeObject<ListSchedule>(response1);

                    HashSet<Schedule> hash2 = new HashSet<Schedule>(E2.data);

                    foreach (Schedule schedule in hash2)
                    {
                        List<EventAPI> eventList = new HashSet<EventAPI>(E1.data).Where(even => even.eid == schedule.eid).ToList();
                        EventClass eventObject = new EventClass(schedule, eventList.First());
                        eve.Add(eventObject);
                    }
                }
                catch
                {
                    //Do nothing
                }

                return eve;
            }
        }

        private async void EventsList_Loaded(object sender, RoutedEventArgs e)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            DatabaseHelperClass db = new DatabaseHelperClass();
            if (RefInBack)
            {
                RefInBack = false;

                if (roamingSettings.Values["Favs"] != null)
                {
                    Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());
                }

                db.DeleteAllEvents();
                List<EventClass> listevents = new List<EventClass>();
                listevents = await GetEventsAPIAsync();
                db.Insert(listevents);

                List<EventClass> list = new List<EventClass>();
                list = db.ReadEvents();

                foreach (EventClass eve in list)
                {
                    if (Favs.Contains(eve.id))
                        eve.Fav_Image = "";
                    else
                        eve.Fav_Image = "";
                }

                List<EventClass> Day1Events = new List<EventClass>();
                List<EventClass> Day2Events = new List<EventClass>();
                List<EventClass> Day3Events = new List<EventClass>();
                List<EventClass> Day4Events = new List<EventClass>();

                Day1Events = list.Where(p => p.Day == "1").ToList();
                Day2Events = list.Where(p => p.Day == "2").ToList();
                Day3Events = list.Where(p => p.Day == "3").ToList();
                Day4Events = list.Where(p => p.Day == "4").ToList();

                ObservableCollection<EventClass> Day1_Events = new ObservableCollection<EventClass>(Day1Events);
                ObservableCollection<EventClass> Day2_Events = new ObservableCollection<EventClass>(Day2Events);
                ObservableCollection<EventClass> Day3_Events = new ObservableCollection<EventClass>(Day3Events);
                ObservableCollection<EventClass> Day4_Events = new ObservableCollection<EventClass>(Day4Events);

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

                Days.Clear();

                this.Days.Add(day1);
                this.Days.Add(day2);
                this.Days.Add(day3);
                this.Days.Add(day4);

                (sender as ListView).ItemsSource = Days;
            }
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

        public void RefreshLayout()
        {
                if (Filter_Fav.SelectedIndex == 1)
                {
                    Day dayfav = new Day();
                    dayfav.day = "";

                    ObservableCollection<EventClass> FavEvents = new ObservableCollection<EventClass>();


                    var roamingSettings = ApplicationData.Current.RoamingSettings;
                    if (roamingSettings.Values["Favs"] != null)
                    {
                        Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());
                    }

                    if (Favs.Count == 0)
                        dayfav.day = "No Favourites Added Yet..";

                    else
                    {
                        DatabaseHelperClass db = new DatabaseHelperClass();

                        foreach (int id in Favs)
                        {
                            if (db.ReadEventById(id) != null)
                            {
                                FavEvents.Add(db.ReadEventById(id));
                            }
                        }

                        foreach (EventClass eve in FavEvents)
                        {
                            eve.Fav_Image = "";
                        }
                    }
                    dayfav.Events = FavEvents;

                    Favourites.Clear();
                    Favourites.Add(dayfav);
                    MyPivot.ItemsSource = Favourites;

                }
                else
                {
                    AssignItemSource();
                    MyPivot.ItemsSource = Days;

                }
        }

        public void AssignItemSource()
        {
            DatabaseHelperClass db = new DatabaseHelperClass();
            List<EventClass> list = new List<EventClass>();
            list = db.ReadEvents();

            var roamingSettings = ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values["Favs"] != null)
                Favs = Deserialize<List<int>>(roamingSettings.Values["Favs"].ToString());

            foreach (EventClass eve in list)
            {
                if (Favs.Contains(eve.id))
                    eve.Fav_Image = "";
                else
                    eve.Fav_Image = "";
            }

            List<EventClass> Day1Events = new List<EventClass>();
            List<EventClass> Day2Events = new List<EventClass>();
            List<EventClass> Day3Events = new List<EventClass>();
            List<EventClass> Day4Events = new List<EventClass>();

            Day1Events = list.Where(p => p.Day == "1").ToList();
            Day2Events = list.Where(p => p.Day == "2").ToList();
            Day3Events = list.Where(p => p.Day == "3").ToList();
            Day4Events = list.Where(p => p.Day == "4").ToList();

            ObservableCollection<EventClass> Day1_Events = new ObservableCollection<EventClass>(Day1Events);
            ObservableCollection<EventClass> Day2_Events = new ObservableCollection<EventClass>(Day2Events);
            ObservableCollection<EventClass> Day3_Events = new ObservableCollection<EventClass>(Day3Events);
            ObservableCollection<EventClass> Day4_Events = new ObservableCollection<EventClass>(Day4Events);

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

            Days.Clear();

            this.Days.Add(day1);
            this.Days.Add(day2);
            this.Days.Add(day3);
            this.Days.Add(day4);
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
                (sender as AppBarButton).AllowFocusOnInteraction = true;
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
       
    }
}
