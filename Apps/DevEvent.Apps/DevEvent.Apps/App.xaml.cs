using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using DevEvent.Apps.Pages;

namespace DevEvent.Apps
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

        public static IAuthenticate Authenticator { get; private set; }

        public App()
        {
            InitializeComponent();

            Authenticator = DependencyService.Get<IAuthenticate>();
            MainPage = new DevEvent.Apps.Pages.MasterPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
