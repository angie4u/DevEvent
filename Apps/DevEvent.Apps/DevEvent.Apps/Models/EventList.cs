using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Apps.Models
{
    public class EventList
    {        
        public int ID { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventStartDay { get; set; }
        public DateTime EventEndDay { get; set; }
        public string Venue { get; set; }
        public double locationX { get; set; }
        public double locationY { get; set; }
        public string Audience { get; set; }
        public string Description { get; set; }
        public string Agenda { get; set; }
        public string Speaker { get; set; }
        public string RegistrationURL { get; set; }
        public string ContentsURL { get; set; }
        public int ImageNumber { get; set; }
        


    }
}
