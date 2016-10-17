using DevEvent.Data.Models;
using DevEvent.Data.Services;
using DevEvent.Data.ViewModels;
using DevEvent.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DevEvent.Web.Controllers
{
    public class EventsController : ApiController
    {
        private IEventService EventService;
        
        public EventsController(IEventService eventService)
        {
            this.EventService = eventService;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<EventListrResponse> Get([FromUri]EventListFilterViewModel model)
        {
            try
            {
                var response = new EventListrResponse();

                response.TotalCount = await this.EventService.GetEventCountAsync(model.filter);
                response.Limit = model.limit;
                response.Offset = model.offset;
                // Total Count 를 같이 줘야 함. 
                response.Events = await this.EventService.GetEventListAsync(model);

                return response;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        // GET api/<controller>/5
        public async Task<EventDetailViewModel> Get(int id)
        {
            try
            {
                var item = await this.EventService.GetEventDetailAsync(id);
                return item;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]EventDetailViewModel model)
        {
            try
            {
                await this.EventService.UpdateEventAsync(model);
            }
            catch(ArgumentException aex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, aex.Message));
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        // PUT api/<controller>/5
        [HttpPost]
        public async Task Put(int id, [FromBody]EventDetailViewModel model)
        {
            try
            {
                await this.EventService.AddEventAsync(model);
            }
            catch (ArgumentException aex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, aex.Message));
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        // DELETE api/<controller>/5
        public async Task Delete(int id)
        {
            try
            {
                await this.EventService.DeleteEventAsync(id);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
        }
    }
}