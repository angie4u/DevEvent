using DevEvent.Apps.Services;
using System;
using System.Collections.Generic;
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


        }
    }
}
