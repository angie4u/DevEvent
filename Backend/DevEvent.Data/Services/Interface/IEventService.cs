using DevEvent.Data.Models;
using DevEvent.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public interface IEventService
    {
        Task<IList<EventListViewModel>> GetEventListAsync(EventListFilterViewModel filter);

        Task<EventDetailViewModel> GetEventDetailAsync(long id);

        Task<int> GetEventCountAsync(EventListFilter filter);

        Task DeleteEventAsync(int id);

        Task<long> AddEventAsync(EventDetailViewModel model);

        Task<long> UpdateEventAsync(EventDetailViewModel model);
    }
}
