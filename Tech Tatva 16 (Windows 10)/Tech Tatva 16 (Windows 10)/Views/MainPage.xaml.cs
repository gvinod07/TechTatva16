using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tech_Tatva_16__Windows_10_.Classes;
using Tech_Tatva_16__Windows_10_.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Tech_Tatva_16__Windows_10_
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Frame contentFrame;
        Windows.Storage.ApplicationDataContainer roamingSettings =
       Windows.Storage.ApplicationData.Current.RoamingSettings;
        Windows.Storage.StorageFolder roamingFolder =
            Windows.Storage.ApplicationData.Current.RoamingFolder;

        Popup popup;

        public static MainPage Instance { get; private set; }
        bool flag;

        public List<string> eventnames = new List<string>();

        public MainPage(Frame frame)
        {
            
            this.contentFrame = frame;
            this.InitializeComponent();
            this.HamburgerMenu.Content = frame;

            Instance = this;

            popup = new Popup();
            DatabaseHelperClass db = new DatabaseHelperClass();
            List<EventClass> list = db.ReadEvents();

            foreach(EventClass eve in list)
            {
                eventnames.Add(eve.Name);
            }

            var update = new Action(() =>
             {
                 SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
                 // update radiobuttons after frame navigates 
                 var type = frame.CurrentSourcePageType;


                 if (type == typeof(EventsPage))
                 {
                     Events_Button.IsChecked = true;
                     if(AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "EVENTS";
                         this.HamburgerMenu.IsPaneOpen = false;

                         

                     }
                 }
                 if (type == typeof(ResultsPage))
                 {
                     Results_Button.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "RESULTS";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }
                 if (type == typeof(InstaPage))
                 {
                     Insta_Button.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "INSTAGRAM";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }

                 if (type == typeof(SettingsPage))
                 {
                     SettingsButton.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "SETTINGS";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }

                 if (type == typeof(AboutPage))
                 {
                     SettingsButton.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "ABOUT US";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }

                 if (type == typeof(DevelopersPage))
                 {
                     SettingsButton.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "DEVELOPERS";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }

                 if (type == typeof(CategoriesPage))
                 {
                     CategoriesButton.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "CATEGORIES";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }

                 if (type == typeof(OnlineEventsPage))
                 {
                     Online_Button.IsChecked = true;
                     if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                     {
                         this.Title.Text = "ONLINE EVENTS";
                         this.HamburgerMenu.IsPaneOpen = false;
                     }
                 }

                 SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = contentFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

              });
            frame.Navigated += (s, e) => update();
            this.Loaded += (s, e) => update();
            this.DataContext = this;

            if (this.HamburgerMenu.IsPaneOpen)
            {
                Line1.Visibility = Visibility.Visible;
                Line2.Visibility = Visibility.Visible;

                if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    Search_Textbox.Visibility = Visibility.Visible;
                    Search_Button.Visibility = Visibility.Collapsed;
                }

            }

            else
            {
                Line1.Visibility = Visibility.Collapsed;
                Line2.Visibility = Visibility.Collapsed;

                if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    Search_Textbox.Visibility = Visibility.Collapsed;
                    Search_Button.Visibility = Visibility.Visible;
                }
            }

            this.HamburgerMenu.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, IsPaneOpenPropertyChanged);
        }

        private void IsPaneOpenPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if(this.HamburgerMenu.IsPaneOpen)
            {
                Line1.Visibility = Visibility.Visible;
                Line2.Visibility = Visibility.Visible;

                if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    Search_Textbox.Visibility = Visibility.Visible;
                    Search_Button.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                Line1.Visibility = Visibility.Collapsed;
                Line2.Visibility = Visibility.Collapsed;

                if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    Search_Textbox.Visibility = Visibility.Collapsed;
                    Search_Button.Visibility = Visibility.Visible;
                }
            }
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(contentFrame == null)
            {
                return;
            }

            if (this.popup.IsOpen == true)
            {
                e.Handled = true;
                HidePopup();
            }

            if (contentFrame.SourcePageType == typeof(InstaPage) && InstaPage.Instance.PivotPosition() == 1)
            {
                InstaPage.Instance.GoBack();
                e.Handled = true;
            }

            else if (contentFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                contentFrame.GoBack();
            }
        }
 
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            HamburgerMenu.IsPaneOpen = !HamburgerMenu.IsPaneOpen;
        }

        private void Events_Button_Checked(object sender, RoutedEventArgs e)
        {
            if(contentFrame.SourcePageType != typeof(EventsPage))
                this.contentFrame.Navigate(typeof(EventsPage));
        }

        private void Results_Button_Checked(object sender, RoutedEventArgs e)
        {
           
            if (contentFrame.SourcePageType != typeof(ResultsPage))
                this.contentFrame.Navigate(typeof(ResultsPage));
        }

        private void Insta_Button_Checked(object sender, RoutedEventArgs e)
        {

            if (contentFrame.SourcePageType != typeof(InstaPage))
                this.contentFrame.Navigate(typeof(InstaPage));
        }

        private void Online_Button_Checked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.SourcePageType != typeof(OnlineEventsPage))
                this.contentFrame.Navigate(typeof(OnlineEventsPage));
        }

        private void SettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.SourcePageType != typeof(SettingsPage))
                this.contentFrame.Navigate(typeof(SettingsPage));
        }

        public void ShowPopup()
        {
            ErrorPopup err = new ErrorPopup();
            err.Height = this.ActualHeight;
            err.Width = this.ActualWidth;
            this.popup.Child = err;

            flag = HamburgerMenu.IsPaneOpen;
            this.HamburgerMenu.IsPaneOpen = false;
            
            popup.IsOpen = true;
            
            this.Opacity = 0.2;
            
        }

        public void HidePopup()
        {
            this.HamburgerMenu.IsPaneOpen = flag;
            this.popup.IsOpen = false;
            this.Opacity = 1;
        }

        private void Refresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (contentFrame.SourcePageType == typeof(InstaPage))
            {
                HidePopup();
                contentFrame.Navigate(typeof(InstaPage));
            }
            else if(contentFrame.SourcePageType == typeof(EventsPage))
            {
                HidePopup();
                contentFrame.Navigate(typeof(EventsPage));
            }
            else if (contentFrame.SourcePageType == typeof(ResultsPage))
            {
                HidePopup();
                contentFrame.Navigate(typeof(ResultsPage));
            }
        }

        private void CategoriesButton_Checked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.SourcePageType != typeof(CategoriesPage))
                this.contentFrame.Navigate(typeof(CategoriesPage));
        }

        private void Search_Button_Checked(object sender, RoutedEventArgs e)
        {
            Search_Button.IsChecked = false;
            HamburgerMenu.IsPaneOpen = true;
            Search_Textbox.Focus(FocusState.Keyboard);
        }

        private void Search_Textbox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var autoSuggestBox = sender as AutoSuggestBox;
            var filtered = eventnames.Where(p => p.StartsWith(autoSuggestBox.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
            autoSuggestBox.ItemsSource = filtered;
        }

        private void FontIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Search_Icon.Visibility = Visibility.Collapsed;
            Title.Visibility = Visibility.Collapsed;
            Search_Textbox.Visibility = Visibility.Visible;
            Search_Textbox.Focus(FocusState.Keyboard);
        }

        private void Search_Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            Search_Icon.Visibility = Visibility.Visible;
            Title.Visibility = Visibility.Visible;
            (sender as AutoSuggestBox).Visibility = Visibility.Collapsed;
        }

        private void Search_Textbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if ((args.QueryText as string) == "Harambe" || (args.QueryText as string) == "harambe")
            {
                this.contentFrame.Navigate(typeof(EasterEggPage));
                this.HamburgerMenu.IsPaneOpen = false;
            }
            else
                this.contentFrame.Navigate(typeof(EventsPage), (args.QueryText as string));
        }
    }
}
