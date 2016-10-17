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
using System.Threading.Tasks;
using Tech_Tatva__16.Classes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.Phone.UI.Input;
using System.Collections.ObjectModel;
using Windows.Networking.Connectivity;
using Windows.Storage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Tech_Tatva__16.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        Popup errorpop;
        Popup instapop;
        Popup searchpopup;
        StatusBar statusbar = StatusBar.GetForCurrentView();
        Insta insta = new Insta();

        private bool RefInBack = false;


        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            Application.Current.UnhandledException += Current_UnhandledException;

            errorpop = new Popup();
            instapop = new Popup();
            searchpopup = new Popup();

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

        }

        private void Current_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message.ToString());
        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            this.LayoutRoot.Opacity = 1;

            if (this.instapop.IsOpen == true)
            {
                this.instapop.IsOpen = false;
                await statusbar.ShowAsync();
                CmdBar.Visibility = Visibility.Visible;

                e.Handled = true;
                return;
            }

            if (this.searchpopup.IsOpen == true)
            {
                this.searchpopup.IsOpen = false;
                e.Handled = true;
                return;
            }
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
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            PPanel.Visibility = Visibility.Visible;
            DatabaseHelperClass db = new DatabaseHelperClass();
            List<BitmapImage> bmi = new List<BitmapImage>();
            List<Results> results = new List<Results>();

            BitmapImage bit = new BitmapImage();
            bit.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bit.UriSource = new Uri("ms-appx:///Assets/back.jpg");

            for (int i = 0; i < 9; i++)
                bmi.Add(bit);

            //Checking Network
            bool isConnected = NetworkInterface.GetIsNetworkAvailable();
            if (isConnected)
            {
                // Do Nothing    
            }
            else
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkConnectivityLevel connection = InternetConnectionProfile.GetNetworkConnectivityLevel();
                if (connection == NetworkConnectivityLevel.None || connection == NetworkConnectivityLevel.LocalAccess)
                {
                    isConnected = false;
                }
            }
            var roamingSettings = ApplicationData.Current.RoamingSettings;

            //Nettwork availability checked

            if (roamingSettings.Values["First"].ToString() == "0")
            {
                if (!isConnected)
                {
                    if ((db.ReadEvents() as List<EventClass>).Count == 0)
                        ShowPopupAsync();
                    else
                        HidePopupAsync();
                }
                else
                {
                    //Checking Database
                    if ((db.ReadEvents() as List<EventClass>).Count == 0)
                    {
                        //Start Of EventsAPI call
                        db.DeleteAllEvents();
                        List<EventClass> listevents = new List<EventClass>();
                        listevents = await GetEventsAPIAsync();

                        db.Insert(listevents);
                        //End of Events API Call
                    }
                    else
                    {
                        RefInBack = true;
                    }

                        //Start Of Insta API Call
                    insta = await GetInstaAsync();
                    bmi.Clear();
                    foreach (Datum d in insta.data)
                    {
                        BitmapImage b = new BitmapImage();
                        b.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        b.UriSource = new Uri(d.images.thumbnail.url);

                        bmi.Add(b);
                    }
                    //End Of Insta API call and formatting

                    results = await GetResultsAsync(); //Results API Call
                    roamingSettings.Values["First"] = "1";

                }

            }
            else
            {
                insta = await GetInstaAsync();
                bmi.Clear();
                foreach (Datum d in insta.data)
                {
                    BitmapImage b = new BitmapImage();
                    b.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    b.UriSource = new Uri(d.images.thumbnail.url);

                    bmi.Add(b);
                }

                results = await GetResultsAsync();
            }

            List<EventClass> l = new List<EventClass>();
            l = db.ReadEvents();

            List<EventClass> Day1_Events = new List<EventClass>();
            List<EventClass> Day2_Events = new List<EventClass>();
            List<EventClass> Day3_Events = new List<EventClass>();
            List<EventClass> Day4_Events = new List<EventClass>();

            Day1_Events = (l.Where(p => p.Day == "1").ToList()).OrderBy(eve => eve.Name).ToList();
            Day2_Events = (l.Where(p => p.Day == "2").ToList()).OrderBy(eve => eve.Name).ToList();
            Day3_Events = (l.Where(p => p.Day == "3").ToList()).OrderBy(eve => eve.Name).ToList();
            Day4_Events = (l.Where(p => p.Day == "4").ToList()).OrderBy(eve => eve.Name).ToList();

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



            this.defaultViewModel["Days"] = list;
            this.defaultViewModel["Insta"] = bmi;
            this.defaultViewModel["Results"] = results;

            PPanel.Visibility = Visibility.Collapsed;

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
            this.errorpop.IsOpen = false;
            this.searchpopup.IsOpen = false;
            this.instapop.IsOpen = false;

            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Day_Clicked(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(EventsPage), e.ClickedItem as Day);
        }

        private async void Insta_Clicked(object sender, ItemClickEventArgs e)
        {
            if ((e.ClickedItem as BitmapImage).UriSource.ToString() == "ms-appx:/Assets/back.jpg")
            {
                ShowPopupAsync();
            }
            else
            {
                HidePopupAsync();
                Datum d = new Datum();

                foreach (Datum datum in insta.data)
                {
                    if (datum.images.thumbnail.url.Equals((e.ClickedItem as BitmapImage).UriSource.ToString()))
                    {
                        d = datum;
                        break;
                    }
                }

                await statusbar.HideAsync();


                this.LayoutRoot.Opacity = 0;
                CmdBar.Visibility = Visibility.Collapsed;

                InstaOverlay ovr = new InstaOverlay();
                ovr.Height = this.ActualHeight;
                ovr.Width = this.ActualWidth;
                this.instapop.Child = ovr;
                ovr.DataContext = d;
                this.instapop.IsOpen = true;
            }
        }

        private void Abt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void Dev_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DevelopersPage));
        }

        private void Cat_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CategoriesPage));
        }

        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            SearchPopup s = new SearchPopup();
            s.Width = this.ActualWidth;
            s.Height = this.ActualHeight;
            searchpopup.Child = s;

            searchpopup.IsOpen = true;
        }

        private void Fav_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavouritesPage));
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

                    foreach(Schedule schedule in hash2)
                    {
                        List<EventAPI> eventList = new HashSet<EventAPI>(E1.data).Where(even => even.eid == schedule.eid).ToList();
                        EventClass eventObject = new EventClass(schedule, eventList.First());
                        eve.Add(eventObject);
                    }
                                  
                }
                catch (Exception e)
                {
                    //Do nothing
                }

                return eve;
            }
        }

        private async Task<Insta> GetInstaAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Insta insta = new Insta();
                    var response = await client.GetStringAsync("https://api.instagram.com/v1/tags/techtatva16/media/recent?access_token=630237785.f53975e.8dcfa635acf14fcbb99681c60519d04c&count=9");
                    insta = JsonConvert.DeserializeObject<Insta>(response);
                    return insta;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<Results>> GetResultsAsync()
        {
            List<Results> res = new List<Results>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    ListResultAPI listres = new ListResultAPI();
                    var response = await client.GetStringAsync("http://api.mitportals.in/results");
                    listres = JsonConvert.DeserializeObject<ListResultAPI>(response);

                    res = App.MergeResults(listres);
                }
                catch
                {
                    //Do Nothing
                }

                return res;
            }
        }

        private async void ShowPopupAsync()
        {
            await statusbar.HideAsync();
            CmdBar.Visibility = Visibility.Collapsed;
            this.LayoutRoot.Opacity = 0.2;
            ErrorOverlay ovr = new ErrorOverlay();
            ovr.Width = this.ActualWidth;
            ovr.Height = this.ActualHeight;
            this.errorpop.Child = ovr;
            this.errorpop.IsOpen = true;
        }

        private async void HidePopupAsync()
        {
            this.LayoutRoot.Opacity = 1;
            CmdBar.Visibility = Visibility.Visible;
            await statusbar.ShowAsync();
        }

        private void Result_Clicked(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ResultsPage), (e.ClickedItem as Results));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["First"] = "0";

            Frame.Navigate(typeof(MainPage));
        }

        private async void ListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseHelperClass db = new DatabaseHelperClass();
            if (RefInBack)
            {
                //Start Of EventsAPI call
                db.DeleteAllEvents();
                List<EventClass> listevents = new List<EventClass>();
                listevents = await GetEventsAPIAsync();

                db.Insert(listevents);
                //End of Events API Call



                List<EventClass> l = new List<EventClass>();
                l = db.ReadEvents();

                List<EventClass> Day1_Events = new List<EventClass>();
                List<EventClass> Day2_Events = new List<EventClass>();
                List<EventClass> Day3_Events = new List<EventClass>();
                List<EventClass> Day4_Events = new List<EventClass>();

                Day1_Events = (l.Where(p => p.Day == "1").ToList()).OrderBy(eve => eve.Name).ToList();
                Day2_Events = (l.Where(p => p.Day == "2").ToList()).OrderBy(eve => eve.Name).ToList();
                Day3_Events = (l.Where(p => p.Day == "3").ToList()).OrderBy(eve => eve.Name).ToList();
                Day4_Events = (l.Where(p => p.Day == "4").ToList()).OrderBy(eve => eve.Name).ToList();

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

                RefInBack = false;

                this.defaultViewModel["Days"] = list;
                (sender as ListView).ItemsSource = this.defaultViewModel["Days"];
            }

        }
    }
}
