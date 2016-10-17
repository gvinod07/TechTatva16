using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Tech_Tatva_16__Windows_10_.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ResultsPage : Page
    {
        public List<Results> Result= new List<Results>();

        public ResultsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Result = await GetResultsAsync(); //Results API Call

            if (Result.Count == 0 || Result == null)
                NoResult.Visibility = Visibility.Visible;
            Results.ItemsSource = Result;
            PPanel.Visibility = Visibility.Collapsed;

        }

        private void Event_Clicked(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                ListViewItem _Item = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                var _Children = AllChildren(_Item);
                var _Name = "Table";
                var _Control = (StackPanel)_Children.First(c => c.Name == _Name);

                _Control.Visibility = Visibility.Visible;

                // you will get slected item here. Use that item to get listbox item
            }
            foreach (var item in e.RemovedItems)
            {
                ListViewItem _Item = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                var _Children = AllChildren(_Item);
                var _Name = "Table";
                var _Control = (StackPanel)_Children.First(c => c.Name == _Name);

                _Control.Visibility = Visibility.Collapsed;
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

        private void Results_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
