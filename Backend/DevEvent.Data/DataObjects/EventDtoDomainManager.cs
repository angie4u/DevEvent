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
            IQueryable<MobileEvent> query = this.Context.Set<Event>().ProjectTo<MobileEvent>();
            query = TableUtils.ApplyDeletedFilter(query, this.IncludeDeleted);
            return query;
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
            long eventid = GetKey<long>(mobileid);
            return LookupEntity(c => c.EventId == eventid);
        }

        public override async Task<MobileEvent> UpdateAsync(string mobileid, Delta<MobileEvent> patch)
        {
            long id = GetKey<long>(mobileid);

            Event existingEvent = await this.DbContext.Set<Event>().FindAsync(id);

            if (existingEvent == null)
            {
                throw new HttpResponseException(this.Request.CreateNotFoundResponse());
            }

            MobileEvent existingMobileEvent = Mapper.Map<Event, MobileEvent>(existingEvent);
            patch.Patch(existingMobileEvent);
            Mapper.Map<MobileEvent, Event>(existingMobileEvent, existingEvent);

            await this.SubmitChangesAsync();

            MobileEvent updatedMobileEvent = Mapper.Map<Event, MobileEvent>(existingEvent);

            return updatedMobileEvent;
        }

        public string sId { get; set; }
    }
}
