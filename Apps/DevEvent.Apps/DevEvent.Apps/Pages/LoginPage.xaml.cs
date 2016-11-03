using DevEvent.Apps.Models;
using DevEvent.Apps.Services;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DevEvent.Apps.Pages
{
    public partial class LoginPage : ContentPage
    {
        //bool authenticated = false;
        MobileEventManager manager;

        public LoginPage()
        {
            InitializeComponent();
        }
        private async void OnMicrosoftAccountLoginButtonClicked(object sender, EventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        private async void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Google);
        }

        private async void OnFacebookLoginButtonClicked(object sender, EventArgs e)
        {
            await Login(MobileServiceAuthenticationProvider.Facebook);
        }

        private async Task Login(MobileServiceAuthenticationProvider provider)
        {
            try
            {
                if (App.Authenticator != null)
                {
                    App.IsAuthenticated = await App.Authenticator.AuthenticateAsync(provider);
                }

                if (App.IsAuthenticated)
                {
                    // 로그인이 성공하고 네트워크가 연결되어 있다면 서버와 데이터 Sync 
                    if (CrossConnectivity.Current.IsConnected == true)
                    {
                        manager = MobileEventManager.DefaultManager;
                        await manager.SyncAsync();
                    }

                    // go back
                    await App.Navigator.PopAsync(true);
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                {
                    messageLabel.Text = "Authentication cancelled by the user";
                }
            }
            catch (Exception)
            {
                messageLabel.Text = "Authentication failed";
            }
        }

    }
}
