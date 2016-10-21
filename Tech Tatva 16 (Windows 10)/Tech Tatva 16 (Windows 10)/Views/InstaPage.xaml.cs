using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Tech_Tatva_16__Windows_10_.Classes;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Tech_Tatva_16__Windows_10_.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstaPage : Page
    {
        public FixedSizeObservableCollection<BitmapImage> bmi9 = new FixedSizeObservableCollection<BitmapImage>(9);
        public FixedSizeObservableCollection<BitmapImage> bmi25 = new FixedSizeObservableCollection<BitmapImage>(25);
        Insta instagram = new Insta();


        public static InstaPage Instance { get; private set; }
        public static Pivot Layout;

        public InstaPage()
        {
            this.InitializeComponent();
            this.Loaded += InstaPage_Loaded;

            Instance = this;
            Layout = this.MyPivot;

            bmi9.CollectionChanged += Bmi9_CollectionChanged;
            
        }



        private void Bmi9_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(bmi9.Count == 9)
            {
                PRing.Visibility = Visibility.Collapsed;
            }
        }

        public int PivotPosition()
        {
            return MyPivot.SelectedIndex;
        }

        public void GoBack()
        {
            MyPivot.SelectedItem = MyPivot.Items[0];
        }

        private void InstaPage_Loaded(object sender, RoutedEventArgs e)
        {
            PRing.Visibility = Visibility.Visible;
            GetInstaAsync();
        }

        private void Insta_Clicked(object sender, ItemClickEventArgs e)
        {
            foreach (Datum d in instagram.data)
            {
                if ((e.ClickedItem as BitmapImage).UriSource.ToString().Equals(d.images.thumbnail.url))
                {
                    (MyPivot.Items[1] as PivotItem).DataContext = d;
                    MyPivot.SelectedItem = MyPivot.Items[1];

                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = new Uri(d.user.profile_picture, UriKind.RelativeOrAbsolute);
                    Propic.ImageSource = bmi;

                    BitmapImage bmi1 = new BitmapImage();
                    bmi1.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

                    string url = d.images.standard_resolution.url;
                    url = url.Replace("s640x640", "1080x1080");
                    bmi1.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                    Img.Source = bmi1;
                }
            }
        }

        private async void GetInstaAsync()
        {
            if(IsInternet())
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Insta insta = new Insta();
                    var response = await client.GetStringAsync("https://api.instagram.com/v1/tags/techtatva16/media/recent?access_token=630237785.f53975e.8dcfa635acf14fcbb99681c60519d04c");
                    insta = JsonConvert.DeserializeObject<Insta>(response);

                    instagram = insta;
                    foreach(Datum d in insta.data)
                    {
                        BitmapImage b = new BitmapImage();
                        b.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        b.UriSource = new Uri(d.images.thumbnail.url);

                        bmi9.Add(b);
                        bmi25.Add(b);
                    }

              
                }
            }
            catch (Exception ex)
            {
               
            }

            else
            {
                MainPage.Instance.ShowPopup();

                PRing.Visibility = Visibility.Collapsed;
            }
        }

        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

    }
}
