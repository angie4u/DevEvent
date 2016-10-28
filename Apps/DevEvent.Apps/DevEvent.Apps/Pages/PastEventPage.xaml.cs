using DevEvent.Apps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DevEvent.Apps.Pages
{
    public partial class PastEventPage : ContentPage
    {
        public PastEventPage()
        {
            InitializeComponent();
            Month.Text = "행사 목록";
            GetEventList();
        }

        async void GetEventList()
        {
            ApiService apiService = new ApiService();
            var list = await apiService.GetMobileEvent();
            
            MyList.ItemsSource = list;
        }

        async void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }
    }
}
