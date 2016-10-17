using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Tech_Tatva_16__Windows_10_.Classes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Tech_Tatva_16__Windows_10_
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Events.sqlite"));//DataBase Name 

        public App()
        {
            this.InitializeComponent();

            if (!CheckFileExists("Events.sqlite").Result)
            {
                using (var db = new SQLiteConnection(DB_PATH))
                {
                    db.CreateTable<EventClass>();
                }
            }

            Windows.Storage.ApplicationDataContainer roamingSettings =
        Windows.Storage.ApplicationData.Current.RoamingSettings;
            Windows.Storage.StorageFolder roamingFolder =
                Windows.Storage.ApplicationData.Current.RoamingFolder;

            roamingSettings.Values["Theme"] = roamingSettings.Values["ThemeNew"];

            if(roamingSettings.Values["Theme"] == null)
            {
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    roamingSettings.Values["Theme"] = "Use System Setting";

                else
                    roamingSettings.Values["Theme"] = "Light";
            }

            if (roamingSettings.Values["Theme"].Equals("Light"))
            {
                this.RequestedTheme = ApplicationTheme.Light;
            }

            else if (roamingSettings.Values["Theme"].Equals("Dark"))
            {
                this.RequestedTheme = ApplicationTheme.Dark;
            }

            
        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
            }
            return false;
        }






        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;


            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    rootFrame.NavigationFailed += OnNavigationFailed;

                    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        //TODO: Load state from previously suspended application
                    }

                // Place the frame in the current Window
                Window.Current.Content = new MainPage(rootFrame);
                if (rootFrame.Content == null)
                        {
                            // When the navigation stack isn't restored navigate to the first page,
                            // configuring the new page by passing required information as a navigation
                            // parameter
                            rootFrame.Navigate(typeof(Views.EventsPage), e.Arguments);
                        }
                // Ensure the current window is active
                Window.Current.Activate();
                    }
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        public static List<Results> MergeResults(ListResultAPI res)
        {
            List<Results> results = new List<Results>();


            List<string> dummynames = new List<string>();
            List<string> names = new List<string>();

            dummynames = res.data.Select(p => p.eve).ToList();
            names = dummynames.Distinct().ToList();

            res.data.GroupBy(n => n.eve);

            for (int i = 0; i < names.Count; i++)
            {
                List<Team> teams = new List<Team>();
                Results result = new Results();

                foreach (ResultAPI resultapi in res.data)
                {
                    if (names[i] == resultapi.eve)
                    {
                        Team team = new Team();
                        team.Teamid = resultapi.tid;
                        team.Round = resultapi.round;
                        team.Position = resultapi.pos;
                        teams.Add(team);
                    }
                }
                result.EventName = names[i];
                result.Image = "ms-appx:///Assets/Square150x150Logo.scale-200.png";
                result.Teams = teams;

                results.Add(result);
            }

            return results;
        }

    }
}
