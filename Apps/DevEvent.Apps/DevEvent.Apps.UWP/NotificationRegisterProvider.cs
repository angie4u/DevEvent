using DevEvent.Apps.Models;
using DevEvent.Apps.UWP;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationRegisterProvider))]
namespace DevEvent.Apps.UWP
{
    public class NotificationRegisterProvider : INotificationRegister
    {
        public async Task Register(string tag)
        {
            // 템플릿과 채널로 Mobile App에 연결된 Notification Hub 에 등록 한다. 
            await MobileEventManager.DefaultManager.CurrentClient.GetPush().RegisterAsync(App.PushChannel.Uri, App.PushTemplate);

            var body = new JArray();
            body.Add(tag);

            // Call the custom API '/api/updatetags/<installationid>' 
            // with the JArray of tags.
            var response = await MobileEventManager.DefaultManager.CurrentClient.InvokeApiAsync("updatetags/" + MobileEventManager.DefaultManager.CurrentClient.InstallationId, body);
        }
    }
}
