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
        public async Task<List<EventList>> GetAllEvent(DateTime today)
        {
            //DateTime today = DateTime.Now;


            List<EventList> eventList = new List<EventList>();
            HttpClient Client = new HttpClient();

            //string param = today.ToString("yyyy-MM-dd");
            string param = "2016-01-01";


            Uri myUri = new Uri("http://sevenstars.azurewebsites.net/EventModels/getThisMonthEvent/" + param);

            HttpResponseMessage response = await Client.GetAsync(myUri);
            string jsonString = await response.Content.ReadAsStringAsync();

            //JObject obj = JObject.Parse(jsonString);
            JArray array = JArray.Parse(jsonString);

            int eventCount = array.Count;

            for (int i = 0; i < eventCount; i++)
            {
                EventList e = new EventList();

                e.ID = array[i].Value<int>("ID");
                e.EventTitle = array[i].Value<string>("EventTitle");
                e.EventStartDay = array[i].Value<DateTime>("EventStartDay");
                e.EventEndDay = array[i].Value<DateTime>("EventEndDay");
                e.Venue = array[i].Value<string>("Venue");
                e.locationX = array[i].Value<double>("locationX");
                e.locationY = array[i].Value<double>("locationY");
                e.Audience = array[i].Value<string>("Audience");
                e.Description = array[i].Value<string>("Description");
                e.Agenda = array[i].Value<string>("Agenda");
                e.Speaker = array[i].Value<string>("Speaker");
                e.RegistrationURL = array[i].Value<string>("RegistrationURL");
                e.ContentsURL = array[i].Value<string>("ContentsURL");
                e.ImageNumber = array[i].Value<int>("ImageNumber");
                
                eventList.Add(e);

            }


            return eventList;
            
        }

        }
    }
