using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevEvent.Data.Models;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace DevEvent.Data.DataObjects
{
    public class EventDtoDomainManager : MappedEntityDomainManager<MobileEvent, Event>
    {
        private ApplicationDbContext DbContext;

        public EventDtoDomainManager(ApplicationDbContext context, HttpRequestMessage request)
            : base(context, request)
        {
            this.DbContext = context;
            Request = request;
        }
        private long GetKey(string mobileid, DbSet<Event> events, HttpRequestMessage request)
        {
            long eventid = events.Where(x => x.Id == mobileid).Select(x => x.EventId).FirstOrDefault();
            if (eventid == 0)
            {
                throw new HttpResponseException(request.CreateNotFoundResponse());
            }
            return eventid;
        }

        /// <summary>
        /// AutoMapper.QueryableExtensions.Extensions.Project 에서 System.MissingMethodException 이 발생하는 오류가 있어 
        /// Query를 재정의 하고 github 원본의 코드를 가져와서 넣고 해결함. 원인은 모름
        /// 관련 github issue: https://github.com/Azure/azure-mobile-apps-net-server/issues/96 
        /// </summary>
        /// <returns></returns>
        public override IQueryable<MobileEvent> Query()
        {
            var query = this.DbContext.Events.Where(x => x.PublishState == PublishState.Published).Select(x => new MobileEvent
            {
                Address = x.Address,
                Audience = x.Audience,
                CreatedAt = x.CreatedAt,
                CreateUserId = x.CreateUserId,
                CreateUserName = x.CreateUser.Name,
                Deleted = x.Deleted,
                Description = x.Description,
                EndDate = x.EndDate,
                EventId = x.EventId,
                FeaturedImageUrl = x.FeaturedImageUrl,
                Id = x.Id,
                IsFavorite = x.FavoriteMobileUsers.Where(s => s.sId == this.sId).Any(),  // 이걸 AutoMapper 설정하지 못해서 수동으로 Mapping
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PublishState = x.PublishState,
                RegistrationUrl = x.RegistrationUrl,
                StartDate = x.StartDate,
                ThumbnailImageUrl = x.ThumbnailImageUrl,
                Title = x.Title,
                UpdatedAt = x.UpdatedAt,
                Venue = x.Venue,
                Version = x.Version
            });
            query = TableUtils.ApplyDeletedFilter(query, this.IncludeDeleted);
            return query;

            //IQueryable<MobileEvent> query = this.Context.Set<Event>().ProjectTo<MobileEvent>();
            //query = TableUtils.ApplyDeletedFilter(query, this.IncludeDeleted);
            //return query;
        }


        protected override T GetKey<T>(string mobileid)
        {
            return (T)(object)GetKey(mobileid, this.DbContext.Events, this.Request);
        }

        public override async Task<bool> DeleteAsync(string mobileid)
        {
            long eventid = GetKey<long>(mobileid);
            return await DeleteItemAsync(eventid);
        }

        public override async Task<MobileEvent> InsertAsync(MobileEvent mobileevent)
        {
            return await base.InsertAsync(mobileevent);
        }

        public override SingleResult<MobileEvent> Lookup(string mobileid)
        {
            var query = GetQuery(mobileid);

            return SingleResult.Create(query);

            //long eventid = GetKey<long>(mobileid);
            //return LookupEntity(c => c.EventId == eventid);
        }

        public override async Task<MobileEvent> UpdateAsync(string mobileid, Delta<MobileEvent> patch)
        {
            var mevent = patch.GetEntity();
            // Get Event 
            var evt = this.DbContext.Events.Include(x => x.FavoriteMobileUsers).Where(x => x.Id == mobileid).FirstOrDefault();
            var existed = evt.FavoriteMobileUsers.Where(x => x.sId == this.sId).Any();

            // Get MobileUser 
            var muser = this.DbContext.MobileUsers.Where(x => x.sId == this.sId).FirstOrDefault();

            if (muser == null)
            {
                throw new ArgumentNullException("There is no MobileUser. (check the sId in the EventDtoDomainManager");
            }

            if (mevent.IsFavorite == true)
            {
                // add
                if (existed == false) evt.FavoriteMobileUsers.Add(muser);
            }
            else
            {
                // remove
                if (existed == true) evt.FavoriteMobileUsers.Remove(muser);
            }

            await this.DbContext.SaveChangesAsync();

            return await GetQuery(mobileid).FirstOrDefaultAsync();
        }

        public string sId { get; set; }

        private IQueryable<MobileEvent> GetQuery(string mobileid)
        {
            return this.DbContext.Events.Include(x => x.CreateUser).Include(x => x.FavoriteMobileUsers).Where(x => x.Id == mobileid).Select(x => new MobileEvent
            {
                Address = x.Address,
                Audience = x.Audience,
                CreatedAt = x.CreatedAt,
                CreateUserId = x.CreateUserId,
                CreateUserName = x.CreateUser.Name,
                Deleted = x.Deleted,
                Description = x.Description,
                EndDate = x.EndDate,
                EventId = x.EventId,
                FeaturedImageUrl = x.FeaturedImageUrl,
                Id = x.Id,
                IsFavorite = x.FavoriteMobileUsers.Where(s => s.sId == this.sId).Any(),  // 이걸 AutoMapper 설정하지 못해서 수동으로 Mapping
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PublishState = x.PublishState,
                RegistrationUrl = x.RegistrationUrl,
                StartDate = x.StartDate,
                ThumbnailImageUrl = x.ThumbnailImageUrl,
                Title = x.Title,
                UpdatedAt = x.UpdatedAt,
                Venue = x.Venue,
                Version = x.Version
            });
        }

    }
}
