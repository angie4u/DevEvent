using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevEvent.Apps.Pages;
using Xamarin.Forms;

namespace DevEvent.Apps.Services
{
    public class NavigationService
    {
        public async void Navigate(string pageName)
        {
            App.Master.IsPresented = false;
            switch (pageName)
            {
                case "MainPage":
                    await Navigate(new MainPage());
                    break;

                case "PastEvent":
                    await Navigate(new PastEventPage());
                    break;
            }
        }

        private static async Task Navigate<T>(T page) where T:Page
        {
            NavigationPage.SetHasBackButton(page, false);
            await App.Navigator.PushAsync(page);
        }
    }
}
