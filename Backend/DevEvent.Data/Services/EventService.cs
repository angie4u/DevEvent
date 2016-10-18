using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEvent.Data.ViewModels;
using DevEvent.Data.Models;
using System.Data.Entity;
using System.IO;
using System.Drawing;

namespace DevEvent.Data.Services
{
    public class EventService : IEventService
    {
        private ApplicationDbContext DbContext;
        private IStorageService StorageService;
        private IThumbnailService ThumbnailService;

        public EventService(ApplicationDbContext dbContext, IStorageService storageService, IThumbnailService thumbnailService)
        {
            this.DbContext = dbContext;
            this.StorageService = storageService;
            this.ThumbnailService = thumbnailService;
        }

        /// <summary>
        /// Event 상세정보 가져오기
        /// </summary>
        /// <param name="id">이벤트 ID</param>
        /// <returns></returns>
        public async Task<EventDetailViewModel> GetEventDetailAsync(long id)
        {
            var item = await DbContext.Events.Include(x => x.CreateUser).Include(x => x.RelatedLinks).Where(x => x.EventId == id).Select(x => new EventDetailViewModel
            {
                Address = x.Address,
                Audience = x.Audience,
                CreateUserId = x.CreateUserId,
                CreateUserName = x.CreateUser.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                FeaturedImageUrl = x.FeaturedImageUrl,
                EventId = x.EventId,
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
                Venue = x.Venue,
                PublishState = x.PublishState
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
            switch (filter.filter)
            {
                case EventListFilter.Following:
                    events = events.Where(x => x.StartDate > now);
                    break;
                case EventListFilter.Former:
                    events = events.Where(x => x.StartDate <= now);
                    break;
            }

            // Paging
            events = events.OrderByDescending(x => x.StartDate).Skip(filter.offset).Take(filter.limit);

            // Select to viewmodel 
            return await events.Select(x => new EventListViewModel
            {
                Description = x.Description,
                EventId = x.EventId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ThumbnailImageUrl = x.ThumbnailImageUrl,
                Title = x.Title,
                PublishState = x.PublishState
            }).ToListAsync();
        }

        /// <summary>
        /// Delete specific Event and it's related links.
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteEventAsync(int id)
        {
            var item = this.DbContext.Events.Where(x => x.EventId == id).FirstOrDefault();
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

            try
            {
                // Delete files 
                if (!string.IsNullOrEmpty(item.FeaturedImageUrl))
                {
                    await this.StorageService.DeleteBlobAsync("images", item.FeaturedImageUrl.Replace(this.StorageService.StorageBaseUrl + "images/", ""));
                }

                if (!string.IsNullOrEmpty(item.ThumbnailImageUrl))
                {
                    await this.StorageService.DeleteBlobAsync("thumbs", item.ThumbnailImageUrl.Replace(this.StorageService.StorageBaseUrl + "thumbs/", ""));
                }
            }
            catch { } // skip error anyway. It will make a trash.

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
                Latitude = model.Latitude,
                Longitude = model.Longitude, 
                PublishState = PublishState.Created,
                RegistrationUrl = model.RegistrationUrl,
                StartDate = model.StartDate,
                Title = model.Title,
                Venue = model.Venue,
            };

            // TODO: RelatedLink,
            // Save Metadata
            this.DbContext.Events.Add(newevent);
            try
            {
                await this.DbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {

            }
            

            // File Save
            var fileName = Path.GetFileName(model.FeaturedImageFile.FileName);
            var guid = Guid.NewGuid().ToString().ToLower();
            await this.StorageService.UploadBlobAsync(model.FeaturedImageFile.InputStream, guid + "/" + fileName, "images");

            var url = Path.Combine(this.StorageService.StorageBaseUrl, "images/" + guid, fileName);
            newevent.FeaturedImageUrl = url;
            
#if DEBUG
            // Make thumbnail and save itto the blob
            Image srcimg = Image.FromStream(model.FeaturedImageFile.InputStream);
            Image thumbimg = this.ThumbnailService.CreateThumbnailImage(150, srcimg, true);

            MemoryStream stream = new MemoryStream();
            thumbimg.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            await this.StorageService.UploadBlobAsync(stream, guid + "/" + fileName, "thumbs");
            var thumburl = Path.Combine(this.StorageService.StorageBaseUrl, "thumbs/" + guid, fileName);
            newevent.ThumbnailImageUrl = thumburl;
#else
            // TODO: Enqueue to make thumbnail
#endif

            // save again with image urls
            await this.DbContext.SaveChangesAsync();

            return newevent.EventId;
        }

        public async Task<long> UpdateEventAsync(EventDetailViewModel model)
        {
            var item = this.DbContext.Events.Where(x => x.EventId == model.EventId).FirstOrDefault();
            if (item == null) throw new ArgumentException("No Event with id " + model.EventId);

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

            return item.EventId;
        }
    }
}
