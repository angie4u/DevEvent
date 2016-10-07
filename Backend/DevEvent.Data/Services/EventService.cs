using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevEvent.Data.ViewModels;
using DevEvent.Data.Models;
using System.Data.Entity;

namespace DevEvent.Data.Services
{
    public class EventService : IEventService
    {
        private ApplicationDbContext DbContext; 

        public EventService(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Event 상세정보 가져오기
        /// </summary>
        /// <param name="id">이벤트 ID</param>
        /// <returns></returns>
        public async Task<EventDetailViewModel> GetEventDetailAsync(long id)
        {
            var item = await DbContext.Events.Include(x => x.CreateUser).Include(x => x.RelatedLinks).Where(x => x.Id == id).Select(x => new EventDetailViewModel
            {
                Address = x.Address,
                Audience = x.Audience,
                CreateUserId = x.CreateUserId,
                CreateUserName = x.CreateUser.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                FeaturedImageUrl = x.FeaturedImageUrl,
                Id = x.Id,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                RegistrationUrl = x.RegistrationUrl,
                RelatedLinks = x.RelatedLinks.Select(r => new EventRelatedLinkViewModel
                {
                    CreatedTime = r.CreatedTime,
                    Description = r.Description,
                    Id = r.Id,
                    LinkType = r.LinkType,
                    Url = r.Url
                }).ToList(),
                StartDate = x.StartDate,
                ThumbnailImageUrl = x.ThumbnailImageUrl,
                Title = x.Title,
                Venue = x.Venue
            }).FirstOrDefaultAsync();

            if (item == null) throw new ArgumentException("No Event with id " + id);

            return item;
        }

        /// <summary>
        /// Filter를 받아서 전체 이벤트 개수를 가져온다. 
        /// </summary>
        /// <param name="filter">EventListFilter (All | Former | Following)</param>
        /// <returns>Count</returns>
        public async Task<int> GetEventCountAsync(EventListFilter filter)
        {
            var now = DateTimeOffset.Now;
            IQueryable<Event> query = DbContext.Events;
            switch (filter)
            {
                case EventListFilter.Following:
                    query = query.Where(x => x.StartDate > now);
                    break;
                case EventListFilter.Former:
                    query = query.Where(x => x.StartDate <= now);
                    break;
            }

            return await query.CountAsync();
        }

        /// <summary>
        /// Get Event list for list view with filter and paging
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IList<EventListViewModel>> GetEventListAsync(EventListFilterViewModel filter)
        {
            var now = DateTimeOffset.Now;
            IQueryable<Event> events = DbContext.Events;

            // former event / following Event 
            switch (filter.Filter)
            {
                case EventListFilter.Following:
                    events = events.Where(x => x.StartDate > now);
                    break;
                case EventListFilter.Former:
                    events = events.Where(x => x.StartDate <= now);
                    break;
            }

            // Paging
            events = events.Skip(filter.offset).Take(filter.limit);

            // Select to viewmodel 
            return await events.Select(x => new EventListViewModel
            {
                Description = x.Description,
                Id = x.Id,
                StartDate = x.StartDate,
                ThumbnailImageUrl = x.ThumbnailImageUrl,
                Title = x.Title
            }).ToListAsync();
        }

        /// <summary>
        /// Delete specific Event and it's related links.
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteEventAsync(int id)
        {
            var item = this.DbContext.Events.Where(x => x.Id == id).FirstOrDefault();
            if (item == null) throw new ArgumentException("No Event with id " + id);

            // Remove related Links
            if (item.RelatedLinks != null && item.RelatedLinks.Count() > 0)
            {
                EventRelatedLink[] links = new EventRelatedLink[item.RelatedLinks.Count];
                item.RelatedLinks.CopyTo(links, 0);
                foreach (var link in links)
                {
                    var rlink = this.DbContext.EventRelatedLinks.Where(x => x.Id == link.Id).FirstOrDefault();
                    if (rlink != null)
                    {
                        this.DbContext.EventRelatedLinks.Remove(rlink);
                    }
                }
            }

            // remove event
            this.DbContext.Events.Remove(item);
            await DbContext.SaveChangesAsync();
        }

        public async Task<long> AddEventAsync(EventDetailViewModel model)
        {
            var newevent = new Event
            {
                Address = model.Address,
                Audience = model.Audience,
                CreateUserId = model.CreateUserId,
                Description = model.Description,
                EndDate = model.EndDate,
                FeaturedImageUrl = model.FeaturedImageUrl, //
                Latitude = model.Latitude,
                Longitude = model.Longitude, 
                PublishState = PublishState.Created,
                RegistrationUrl = model.RegistrationUrl,
                StartDate = model.StartDate,
                Title = model.Title,
                Venue = model.Venue 
            };

            // TODO: RelatedLink, ThumbnailUrl 

            this.DbContext.Events.Add(newevent);
            await this.DbContext.SaveChangesAsync();

            return newevent.Id;
        }

        public async Task<long> UpdateEventAsync(EventDetailViewModel model)
        {
            var item = this.DbContext.Events.Where(x => x.Id == model.Id).FirstOrDefault();
            if (item == null) throw new ArgumentException("No Event with id " + model.Id);

            item.Address = model.Address;
            item.Audience = model.Audience;
            item.CreateUserId = model.CreateUserId;
            item.Description = model.Description;
            item.EndDate = model.EndDate;
            item.FeaturedImageUrl = model.FeaturedImageUrl;
            item.Latitude = model.Latitude;
            item.Longitude = model.Longitude;
            item.PublishState = model.PublishState;
            item.RegistrationUrl = model.RegistrationUrl;
            item.StartDate = model.StartDate;
            item.Title = model.Title;
            item.Venue = model.Venue;

            await this.DbContext.SaveChangesAsync();

            return item.Id;
        }
    }
}
