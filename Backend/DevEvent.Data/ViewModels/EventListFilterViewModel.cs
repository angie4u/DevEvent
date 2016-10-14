using DevEvent.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.ViewModels
{
    public class EventListFilterViewModel
    {
        public int offset { get; set; }

        public int limit { get; set; }

        public EventListFilter filter { get; set; }
    }
}
