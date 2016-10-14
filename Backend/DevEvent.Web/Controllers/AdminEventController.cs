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
                    model.CreateUserId = "f103fa74-3baa-4101-bcb5-0f8cefc4b399";

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

        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Default/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Default/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}