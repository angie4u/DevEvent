using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using DevEvent.Data.DataObjects;
using DevEvent.Data.Models;

namespace DevEvent.Mobile.Controllers
{
    public class MobileEventController : TableController<MobileEvent>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ApplicationDbContext context = new ApplicationDbContext();
            DomainManager = new EventDtoDomainManager(context, Request);
        }

        // GET tables/MobileEvent
        public IQueryable<MobileEvent> GetAllMobileEvent()
        {
            return DomainManager.Query();
            //return Query(); 
        }

        // GET tables/MobileEvent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<MobileEvent> GetMobileEvent(string id)
        {
            return DomainManager.Lookup(id);
        }

        // PATCH tables/MobileEvent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<MobileEvent> PatchMobileEvent(string id, Delta<MobileEvent> patch)
        {
             return DomainManager.UpdateAsync(id, patch);
        }

        // POST tables/MobileEvent
        public async Task<IHttpActionResult> PostMobileEvent(MobileEvent item)
        {
            MobileEvent current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/MobileEvent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteMobileEvent(string id)
        {
             return DeleteAsync(id);
        }
    }
}
