using DevEvent.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevEvent.Web.Models
{
    public class EventListrResponse
    {
        /// <summary>
        /// 전체 개수
        /// </summary>
        public int TotalCount { get; set; }
        
        public int Offset { get; set; }
        public int Limit { get; set; }

        public IEnumerable<EventListViewModel> Events { get; set; }
    }
}