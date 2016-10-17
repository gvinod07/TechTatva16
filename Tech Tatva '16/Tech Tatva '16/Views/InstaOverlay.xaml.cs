using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tech_Tatva__16.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Tech_Tatva__16.Views
{
    public sealed partial class InstaOverlay : UserControl
    {
        public InstaOverlay()
        {
            this.InitializeComponent();
            Loaded += InstaOverlay_Loaded;
        }


        private void InstaOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            Datum d = this.DataContext as Datum;

            BitmapImage bmi = new BitmapImage();
            bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmi.UriSource = new Uri(d.user.profile_picture, UriKind.RelativeOrAbsolute);
            Propic.ImageSource = bmi;

            BitmapImage bmi1 = new BitmapImage();
            bmi1.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmi1.UriSource = new Uri(d.images.standard_resolution.url, UriKind.RelativeOrAbsolute);
            Img.Source = bmi1;
        }
    }
}
