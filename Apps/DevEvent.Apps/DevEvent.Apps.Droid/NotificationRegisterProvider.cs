using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DevEvent.Apps.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationRegisterProvider))]
namespace DevEvent.Apps.Droid
{
    public class NotificationRegisterProvider : INotificationRegister
    {
        public Task Register(string tag)
        {
            throw new NotImplementedException();
        }
    }
}