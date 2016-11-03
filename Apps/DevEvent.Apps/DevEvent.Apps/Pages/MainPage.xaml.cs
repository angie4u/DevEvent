using DevEvent.Apps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevEvent.Apps.Models;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace DevEvent.Apps.Pages
{
    public partial class MainPage : ContentPage
    {
        MobileEventManager manager;

        public MainPage()
        {
            InitializeComponent();
            manager = MobileEventManager.DefaultManager;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ObservableCollection<MobileEvent> items = null;
            try
            {
                // 데이터 Fetch
                items = await manager.GetEventItemsAsync(true);
            }
            catch(UnauthorizedAccessException ex)
            {
                // 데이터 Fetch (Offline)
                //items = await manager.GetEventItemsAsync(false);
                //Navigation.InsertPageBefore(new LoginPage(), this);
                //await Navigation.PopAsync();

                var navigationService = new NavigationService();
                navigationService.Navigate("LoginPage");
            }
            // databinding
            MyList.ItemsSource = items;
        }
     
        //이벤트 정보 넘겨주는 코드 
        async void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var eventDetailPage = new EventDetailPage();               
                eventDetailPage.BindingContext = e.SelectedItem as MobileEvent;
                await Navigation.PushAsync(eventDetailPage);
                           
            }
        }

        async void FavoriteToggled(object sender, ToggledEventArgs e)
        {
            Switch favoriteSwitch = sender as Switch;
            MobileEvent evt = favoriteSwitch.BindingContext as MobileEvent;
            if (evt == null) return;

            // This code is useless. Because two way binding set the value already. 
            //evt.IsFavorite = e.Value;

            try
            {
                // 로컬에 저장
                await manager.SaveTaskAsync(evt);
                // 즉시 씽크 
                await manager.SyncAsync();
            }
            catch(UnauthorizedAccessException uex)
            {
                //Navigation.InsertPageBefore(new LoginPage(), this);
                //await Navigation.PopAsync();

                var navigationService = new NavigationService();
                navigationService.Navigate("LoginPage");
            }
            catch (Exception ex)
            {

            }
        }



    }
}
