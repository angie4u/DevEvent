using Xamarin.Forms;
using DevEvent.Apps.Pages;
using DevEvent.Apps.Models;
using Plugin.Connectivity;

namespace DevEvent.Apps
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        public static IAuthenticate Authenticator { get; private set; }
        public static INotificationRegister NotificationRegister { get; private set; }
        public static bool IsAuthenticated { get; set; }

        MobileEventManager manager;

        public App()
        {
            InitializeComponent();

            Authenticator = DependencyService.Get<IAuthenticate>();
            NotificationRegister = DependencyService.Get<INotificationRegister>();
            MainPage = new DevEvent.Apps.Pages.MasterPage();
            manager = MobileEventManager.DefaultManager;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            // 네트워크가 살아나면 Sync 한다. 
            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (args.IsConnected == true)
                {
                    await manager.SyncAsync();
                }
            };
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
