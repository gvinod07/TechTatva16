using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
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
    public sealed partial class SettingsPage : Page
    {

        Windows.Storage.ApplicationDataContainer roamingSettings =
        Windows.Storage.ApplicationData.Current.RoamingSettings;
        Windows.Storage.StorageFolder roamingFolder =
            Windows.Storage.ApplicationData.Current.RoamingFolder;

        public SettingsPage()
        {
            this.InitializeComponent();
            foreach (RadioButton r in AllRadioButtons(this))
            {

                if (r.Content.ToString().Equals(roamingSettings.Values["Theme"].ToString()))
                    r.IsChecked = true;
                
            }
        }

        public List<RadioButton> AllRadioButtons(DependencyObject parent)
         { 
             var list = new List<RadioButton>(); 
             for (int i = 0; i<VisualTreeHelper.GetChildrenCount(parent); i++) 
             { 
                 var child = VisualTreeHelper.GetChild(parent, i); 
                 if (child is RadioButton) 
                 { 
                     list.Add(child as RadioButton); 
                     continue; 
                 } 
                 list.AddRange(AllRadioButtons(child)); 
             } 
             return list; 
         } 


        private void LightRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckTheme("Light");
            roamingSettings.Values["ThemeNew"] = "Light";
        }

        private void DarkRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckTheme("Dark");
            roamingSettings.Values["ThemeNew"] = "Dark";
        }

        private void DefaultRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            CheckTheme("Default");
            roamingSettings.Values["ThemeNew"] = "Use System Setting";
        }


        private void About_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void CheckTheme(string val)
        {
            if (roamingSettings.Values["Theme"].ToString() != val)
            {
                RestartApp.Visibility = Visibility.Visible;
            }
            else
                RestartApp.Visibility = Visibility.Collapsed;
        }

        private void AppClicked(object sender, RoutedEventArgs e)
        {
            if ((sender as HyperlinkButton).Content.ToString() == "Developers")
                Frame.Navigate(typeof(DevelopersPage));
            else
                Frame.Navigate(typeof(AboutPage));

        }
    }
}
