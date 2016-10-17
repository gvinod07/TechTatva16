using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tech_Tatva__16.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Tech_Tatva__16.Views
{
    public sealed partial class SearchPopup : UserControl
    {
        DatabaseHelperClass db = new DatabaseHelperClass();

        public SearchPopup()
        {
            this.InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<EventClass> list =  db.SearchEvents(SearchBox.Text);
            EventList.ItemsSource = list;
                
        }

        private void EventList_ItemClick(object sender, ItemClickEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(DetailsPage), e.ClickedItem as EventClass);
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            List<EventClass> list = db.SearchEvents(SearchBox.Text);
            EventList.ItemsSource = list;

            if (SearchBox.Text == "Harambe" || SearchBox.Text == "harambe")
                (Window.Current.Content as Frame).Navigate(typeof(EasterEggPage));
        }
    }
}
