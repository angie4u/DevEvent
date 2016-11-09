using DevEvent.Apps.iOS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationRegisterProvider))]
namespace DevEvent.Apps.iOS
{
    public class NotificationRegisterProvider : INotificationRegister
    {
        public Task Register(string tag)
        {
            throw new NotImplementedException();
        }
    }
}
