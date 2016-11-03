using Microsoft.WindowsAzure.MobileServices;
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
        bool authenticated = false;

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
                    authenticated = await App.Authenticator.AuthenticateAsync(provider);
                }

                if (authenticated)
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
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
