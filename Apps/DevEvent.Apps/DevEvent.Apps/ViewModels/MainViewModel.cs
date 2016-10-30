using DevEvent.Apps.Models;
using DevEvent.Apps.Services;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Apps.ViewModels
{
    public class MainViewModel
    {
        NavigationService navigationService;
        ApiService apiService;
        
        MobileEventManager manager;

        public MainViewModel()
        {
            navigationService = new NavigationService();
            apiService = new ApiService();
            
            // Init 
            
            LoadMenu();
            //GetEventList();
        }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<MainViewModel> Main { get; set; }
        //public ObservableCollection<EventListViewModel> Event { get; set; }

        //async void GetEventList()
        //{
        //    manager = MobileEventManager.DefaultManager;
        //    await manager.GetEventItemsAsync();
        //}   
        
        private void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();
            
            Menu.Add(new MenuItemViewModel()
            {
                Icon = "ic_action_home",
                Title = "Home",
                PageName = "MainPage"
            });

            Menu.Add(new MenuItemViewModel()
            {
                Icon = "ic_action_home",
                Title = "PastEvent",
                PageName = "PastEvent"
            });

            Menu.Add(new MenuItemViewModel()
            {
                Icon = "ic_action_home",
                Title = "관심행사",
                PageName = "MainPage"
            });

            Menu.Add(new MenuItemViewModel()
            {
                Icon = "ic_action_facebook",
                Title = "Microsoft Developer",
                PageName = "Microsoft Developer"
            });

        }

        
       
    }
}
