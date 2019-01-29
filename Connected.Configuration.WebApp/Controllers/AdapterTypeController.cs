using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Connected.Common;
using Connected.Configuration.WebApp.ViewModel;
using Connected.DAL.Configuration.Repositories.Fake;
using Connected.DAL.Core;
using Connected.DAL.Core.Configuration.Model;
using Infrastructure.CrossCutting.IocManager;

namespace Connected.Configuration.WebApp.Controllers
{
    public class AdapterTypeController : Controller
    {
        private readonly IUnitOfWork UoW;
        private readonly GenericRepositoryBase<AdapterTypeDIMDTO> _adapterTypesRepo;

        public AdapterTypeController()
        {
            try
            {
                UoW = IoCManager.Instance.ResolveIfRegistered<IUnitOfWork>();
            }
            catch (NotRegisteredException)
            {
                UoW = new UnitOfWorkFake();
            }

            _adapterTypesRepo = UoW.GetRep<AdapterTypeDIMDTO>();
        }

        // GET: AdapterBasic2
        public ActionResult Index()
        {
            List<AdapterTypeViewModel> adaptersTypeList = _adapterTypesRepo.GetAll().Select( x=> new AdapterTypeViewModel()
            {
                Id = x.Id,
                AdapterType = x.AdapterType
            }).ToList();

            return View(adaptersTypeList);
        }

        public ActionResult Details(int id)
        {
            var adapterType = _adapterTypesRepo.GetById(id);
            AdapterTypeViewModel adapterTypeViewModel = new AdapterTypeViewModel()
            {
                AdapterType = adapterType.AdapterType,
                Id = adapterType.Id
            };

            return View(adapterTypeViewModel);
        }

        // GET: AdapterBasic2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdapterBasic2/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "")] AdapterTypeViewModel adapterTypeViewModel)
        {
            try
            {
                using (UoW)
                {
                    _adapterTypesRepo.Insert(new AdapterTypeDIMDTO()
                    {
                        AdapterType = adapterTypeViewModel.AdapterType,
                        Id = adapterTypeViewModel.Id,
                        CreationDate= DateTime.Now,
                        ModifiedDate= DateTime.Now
                    });
                    UoW.Commit();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int? id, bool? saveChangesError = false)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (saveChangesError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Edit failed. Try again, and if the problem persists see your system administrator.";

            var adapter = _adapterTypesRepo.GetById((int)id);
            if (adapter == null) return HttpNotFound();

            return View(new AdapterTypeViewModel()
            {
                AdapterType = adapter.AdapterType,
                Id = adapter.Id
            }) ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] AdapterTypeViewModel adapterTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                using (UoW)
                {
                    _adapterTypesRepo.Update(new AdapterTypeDIMDTO()
                    {
                        Id = adapterTypeViewModel.Id,
                        AdapterType = adapterTypeViewModel.AdapterType
                    });
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit", new { id = adapterTypeViewModel.Id, saveChangesError = true });
        }

        // GET: AdapterBasic2/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdapterBasic2/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, AdapterTypeViewModel adapterTypeViewModel)
        {
            try
            {
                using (UoW)
                {
                    _adapterTypesRepo.Delete(new AdapterTypeDIMDTO() { Id = adapterTypeViewModel.Id});
                    UoW.Commit();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
