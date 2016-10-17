using AutoMapper;
using DevEvent.Data.Models;
using Microsoft.Azure.Mobile.Server;
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
    }
}
