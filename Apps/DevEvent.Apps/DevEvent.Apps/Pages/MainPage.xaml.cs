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
            var items = await manager.GetEventItemsAsync(true);
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

    }
}
