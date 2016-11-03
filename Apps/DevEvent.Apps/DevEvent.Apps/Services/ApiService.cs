using DevEvent.Apps.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Apps.Services
{
    class ApiService
    {
        public async Task<List<MobileEvent>> GetMobileEvent()
        {
            List<MobileEvent> eventList = new List<MobileEvent>();
            HttpClient Client = new HttpClient();
            Uri myUri = new Uri("http://deveventdev.azurewebsites.net/api/Events?model.offset=0&model.limit=100&model.filter=2");
            HttpResponseMessage response = await Client.GetAsync(myUri);
            string jsonString = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(jsonString);
            JArray array = (JArray)obj["Events"];

            int eventCount = array.Count;

            for (int i = 0; i < eventCount; i++)
            {
                MobileEvent e = new MobileEvent();

                e.Id = array[i].Value<string>("Id");
                e.Title = array[i].Value<string>("Title");
                e.StartDate = array[i].Value<DateTime>("StartDate");
                e.EndDate = array[i].Value<DateTime>("EndDate");
                e.Venue = array[i].Value<string>("Venue");
                e.Audience = array[i].Value<string>("Audience");
                e.Description = array[i].Value<string>("Description");
                e.ThumbnailImageUrl = array[i].Value<string>("ThumbnailImageUrl");

                eventList.Add(e);
            }
            return eventList;
            
        }
      

    }
}