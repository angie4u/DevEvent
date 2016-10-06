using DevEvent.Apps.Services;
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



        public MainViewModel()
        {
            navigationService = new NavigationService();
            apiService = new ApiService();

            LoadMenu();

        }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

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
                Title = "지난행사",
                PageName = "MainPage"
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
