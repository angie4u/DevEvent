using System;
using Android.App;
using Xamarin.Forms;
using DevEvent.Apps.Models;
using Microsoft.WindowsAzure.MobileServices;
using DevEvent.Apps.Droid;
using System.Threading.Tasks;
using Android.Webkit;

[assembly: Dependency(typeof(AuthenticationProvider))]
namespace DevEvent.Apps.Droid
{
    public class AuthenticationProvider : IAuthenticate
    {
        MobileServiceUser user;

        public async Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            bool success = false;
            try
            {
                if (user == null)
                {
                    user = await MobileEventManager.DefaultManager.CurrentClient.LoginAsync(Forms.Context, provider);
                    if (user != null)
                    {
                        CreateAndShowDialog(string.Format("You are now logged in - {0}", user.UserId), "Logged in!");
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Authentication failed");
            }
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            bool success = false;
            try
            {
                if (user != null)
                {
                    CookieManager.Instance.RemoveAllCookie();
                    await MobileEventManager.DefaultManager.CurrentClient.LogoutAsync();
                    CreateAndShowDialog(string.Format("You are now logged out - {0}", user.UserId), "Logged out!");
                }
                user = null;
                success = true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Logout failed");
            }

            return success;
        }

        void CreateAndShowDialog(string message, string title)
        {
            var builder = new AlertDialog.Builder(Forms.Context);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetNeutralButton("OK", (sender, args) => { });
            builder.Create().Show();
        }
    }
}