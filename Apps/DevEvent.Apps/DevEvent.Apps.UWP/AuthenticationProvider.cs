using DevEvent.Apps.Models;
using DevEvent.Apps.UWP;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationProvider))]
namespace DevEvent.Apps.UWP
{
    public class AuthenticationProvider : IAuthenticate
    {
        MobileServiceUser user;

        /// <summary>
        /// 로그인 form Windows 10 UWP
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            bool success = false;
            try
            {
                if (user == null)
                {
                    user = await MobileEventManager.DefaultManager.CurrentClient.LoginAsync(provider);
                    if (user != null)
                    {
                        var dialog = new MessageDialog(string.Format("You are now logged in - {0}", user.UserId), "Authentication");
                        await dialog.ShowAsync();
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Authentication Failed");
                await dialog.ShowAsync();
            }
            return success;
        }

        /// <summary>
        /// 로그아웃 from windows 10 UWP
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LogoutAsync()
        {
            bool success = false;
            try
            {
                if (user != null)
                {
                    await MobileEventManager.DefaultManager.CurrentClient.LogoutAsync();

                    var dialog = new MessageDialog(string.Format("You are now logged out - {0}", user.UserId), "Logout");
                    await dialog.ShowAsync();
                }

                user = null;
                success = true;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Logout failed");
                await dialog.ShowAsync();
            }
            return success;
        }
    }
}
