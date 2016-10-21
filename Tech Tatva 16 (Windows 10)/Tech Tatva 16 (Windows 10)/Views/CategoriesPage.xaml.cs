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
    public sealed partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            this.InitializeComponent();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ListCategory list = new ListCategory();
            list = await GetCatAsync();

            foreach (Category cat in list.data)
            {
                cat.Image = "ms-appx:///Assets/Category Icons/TT-" + cat.cname + ".png";
                switch (cat.cname)
                {
                    case "Featured Event-Paper Presentation":
                        cat.Image = "ms-appx:///Assets/Square71x71Logo.scale-100.png";
                        break;
                }
            }

            Categories.ItemsSource = list.data;
        }

        private async Task<ListCategory> GetCatAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    ListCategory list = new ListCategory();
                    var response = await client.GetStringAsync("http://api.mitportals.in/categories/");
                    list = JsonConvert.DeserializeObject<ListCategory>(response);
                    return list;
                }
                catch { }

                return null;
            }
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(CategoriesDetailsPage), (sender as GridView).SelectedItem as Category);
        }
    }

}
