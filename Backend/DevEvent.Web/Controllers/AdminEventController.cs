using DevEvent.Data.Services;
using DevEvent.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DevEvent.Web.Models;

namespace DevEvent.Web.Controllers
{
    public class AdminEventController : Controller
    {
        private IEventService EventService;

        public AdminEventController(IEventService eventService)
        {
            this.EventService = eventService;
        }

        // GET: AdminEvent
        public async Task<ActionResult> Index(EventListFilterViewModel model)
        {
            model.filter = Data.Models.EventListFilter.All;
            model.limit = (model.limit == 0 ? 10 : model.limit);

            var response = new EventListrResponse();
            response.Events = await this.EventService.GetEventListAsync(model);
            response.Limit =  model.limit;
            response.Offset = model.offset;
            response.TotalCount = await this.EventService.GetEventCountAsync(model.filter);

            return View(response);
        }

        public ActionResult Create()
        {
            var model = new EventDetailViewModel();
            model.StartDate = DateTimeOffset.Now;
            model.EndDate = model.StartDate.AddHours(1);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // just test 
                    model.CreateUserId = "8c295a0f-9869-4f2e-b8da-e87ddfec92ee";

                    // created user id 
                    //var userid = User.Identity.GetUserId();
                    //model.CreateUserId = userid;
                    //if (string.IsNullOrEmpty(userid)) throw new AccessViolationException("권한이 없습니다");

                    await this.EventService.AddEventAsync(model);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var evt = await this.EventService.GetEventDetailAsync(id);
                return View(evt);
            }
            catch
            {
                return new HttpNotFoundResult();
            }
        }

        // POST: Default/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EventDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // just test 
                    model.CreateUserId = "8c295a0f-9869-4f2e-b8da-e87ddfec92ee";

                    // created user id 
                    //var userid = User.Identity.GetUserId();
                    //model.CreateUserId = userid;
                    //if (string.IsNullOrEmpty(userid)) throw new AccessViolationException("권한이 없습니다");
                    await this.EventService.UpdateEventAsync(model);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);
        }

        // POST: Default/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await this.EventService.DeleteEventAsync(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}