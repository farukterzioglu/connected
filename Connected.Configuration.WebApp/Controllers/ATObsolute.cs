using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Connected.Common;
using Connected.Configuration.WebApp.ViewModel;
using Connected.DAL.Core;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.Configuration.WebApp.Controllers
{
    public class ATObsolute : Controller
    {
        private readonly IUnitOfWork UoW;
        private readonly GenericRepositoryBase<AdapterTypeDIMDTO> _repo;

        public ATObsolute()
        {
            UoW = IoCManager.Instance.Resolve<IUnitOfWork>();
            _repo = UoW.GetRep<AdapterTypeDIMDTO>();
        }

        // GET: AdapterType
        public ActionResult Index()
        {
            var list = _repo.GetAll().Select( x=> new AdapterTypeViewModel()
            {
                AdapterType = x.AdapterType + " (View Model)",
                Id = x.Id
            });

            return View(list);
        }

        // GET: AdapterType/Details/5
        public ActionResult Details(int id)
        {
            var adapterType = _repo.GetById(id);
            AdapterTypeViewModel adapterTypeViewModel = new AdapterTypeViewModel()
            {
                AdapterType = adapterType.AdapterType,
                Id = adapterType.Id
            };

            return View(adapterTypeViewModel);
        }

        // GET: AdapterType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdapterType/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdapterType/Edit/5
        public ActionResult Edit(int id, bool? saveError = false)
        {
            if(saveError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Edit failed. Try again, and if the problem persists see your system administrator.";

            var adapterType = _repo.GetById(id);
            if (adapterType == null)
                return HttpNotFound();

            AdapterTypeViewModel adapterTypeViewModel = new AdapterTypeViewModel()
            {
                AdapterType = adapterType.AdapterType,
                Id = adapterType.Id
            };

            return View(adapterTypeViewModel);
        }

        // POST: AdapterType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] AdapterTypeViewModel adapterType)
        {
            if (ModelState.IsValid)
            {
                using (UoW)
                {
                    _repo.Update(new AdapterTypeDIMDTO()
                    {
                        Id = adapterType.Id,
                        AdapterType = adapterType.AdapterType
                    });
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Edit", new {id = adapterType.Id, saveError = true});
        }

        // GET: AdapterType/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdapterType/Delete/5
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
