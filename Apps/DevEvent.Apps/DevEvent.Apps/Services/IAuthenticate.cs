using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Apps
{
    public interface IAuthenticate
    {
        Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider);

        Task<bool> LogoutAsync();
    }
}
