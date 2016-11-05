﻿using DevEvent.Apps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevEvent.Apps.Models;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace DevEvent.Apps.Pages
{
    public partial class MainPage : ContentPage
    {
        MobileEventManager manager;

        public MainPage()
        {
            InitializeComponent();
            manager = MobileEventManager.DefaultManager;
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        

        private async void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(!e.IsConnected)
            {
                //message
                await DisplayAlert("Error", "Check for your connection", "OK");
            }
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            
            ObservableCollection<MobileEvent> items = null;

            //네트워크 상태 체크하는 코드 
            //네트워크가 연결되어 있지 않은 경우, 동작은 하지만 await 흐름 처리가 필요 
            if (!CrossConnectivity.Current.IsConnected)
            {
                items = await manager.GetEventItemsAsync(false);
                //Navigation.InsertPageBefore(new LoginPage(), this);
                //await Navigation.PopAsync();

            }

            else
            {
                try
                {
                    // 데이터 Fetch
                    items = await manager.GetEventItemsAsync(true);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // 데이터 Fetch (Offline)
                    //items = await manager.GetEventItemsAsync(false);
                    //Navigation.InsertPageBefore(new LoginPage(), this);
                    //await Navigation.PopAsync();

                    var navigationService = new NavigationService();
                    navigationService.Navigate("LoginPage");
                }
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
