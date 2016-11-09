using DevEvent.Apps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevEvent.Apps.Models;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;

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
                if (CrossConnectivity.Current.IsConnected == true && App.IsAuthenticated == true)
                {
                    // 온라인 이고 인증이 되었으므로 서버에서 (true) 데이터 Fetch
                    items = await manager.GetEventItemsAsync(true);
                }
                else
                {
                    // 오프라인이므로 로컬 DB에서 데이터 가져옴
                    items = await manager.GetEventItemsAsync(false);
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                //var navigationService = new NavigationService();
                //navigationService.Navigate("LoginPage");
            }

            // Databinding
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
                //var navigationService = new NavigationService();
                //navigationService.Navigate("LoginPage");
            }
            catch (Exception ex)
            {

            }

            // Push Notification 등록
            //RegisterPushNotification("event_" + evt.Id);
        }

        private void RegisterPushNotification(string tag)
        {
            App.NotificationRegister.Register(tag);
        }
    }
}
