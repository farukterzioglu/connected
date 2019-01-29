using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Connected.Common;
using Connected.Configuration.WebApp.ViewModel;
using Connected.DAL.Configuration.Repositories.Fake;
using Connected.DAL.Core.Configuration.Model;
using Connected.DAL.Core;
using Infrastructure.CrossCutting.IocManager;

namespace Connected.Configuration.WebApp.Controllers
{
    public static partial class Extensions
    {
        public static String ShowAllErrors(this HtmlHelper helper, String key)
        {
            StringBuilder sb = new StringBuilder();
            if (helper.ViewData.ModelState[key] != null)
            {
                foreach (var e in helper.ViewData.ModelState[key].Errors)
                {
                    TagBuilder div = new TagBuilder("div");
                    div.MergeAttribute("class", "field-validation-error");
                    div.SetInnerText(e.ErrorMessage);
                    sb.Append(div.ToString());
                }
            }
            return sb.ToString();
        }

        public static String ShowAllErrors(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, ModelState> modelState in helper.ViewData.ModelState)
            {
                foreach (var e in modelState.Value.Errors)
                {
                    TagBuilder div = new TagBuilder("div");
                    div.MergeAttribute("class", "field-validation-error");
                    div.SetInnerText(e.ErrorMessage);
                    sb.Append(div.ToString());
                }
            }
            return sb.ToString();
        }
    }

    public class AdapterBasicController : Controller
    {
        private readonly IUnitOfWork UoW;
        private readonly GenericRepositoryBase<AdapterBasicDTO> _repo;
        private readonly GenericRepositoryBase<AdapterTypeDIMDTO> _adapterTypesRepo;

        public AdapterBasicController()
        {
            try
            {
                UoW = IoCManager.Instance.ResolveIfRegistered<IUnitOfWork>();

                _repo = UoW.GetRep<AdapterBasicDTO>();
                _adapterTypesRepo = UoW.GetRep<AdapterTypeDIMDTO>();
            }
            catch (NotRegisteredException)
            {
                UoW = new UnitOfWorkFake();
            }
        }

        private List<AdapterTypeViewModel> GetAdapterTypes()
        {
            var types = _adapterTypesRepo.GetAll().Select(x => new AdapterTypeViewModel()
            {
                Id = x.Id,
                AdapterType = x.AdapterType
            }).ToList();

            return types;
        }

        //private void SetAdapterType2(int? adapterTypeId = null)
        //{
        //    var types = _adapterTypesRepo.GetAll();

        //    ViewBag.AdapterTypes = adapterTypeId != null ? 
        //        new SelectList(types, "Id", "AdapterType", adapterTypeId) : 
        //        new SelectList(types, "Id", "AdapterType");
        //}

        // GET: AdapterBasic

        public ActionResult Index()
        {
            IEnumerable<AdapterBasicViewModel> adaptersList = _repo.GetAll().Select(x => new AdapterBasicViewModel()
            {
                Id = x.Id,
                AdapterName = x.AdapterName, 
                AdapterTypeId = x.AdapterTypeId,
                IsActive = x.IsActive,
                ModifiedDate = x.ModifiedDate,
                RegistrationDate = x.RegistrationDate,
                AdapterTypeList = GetAdapterTypes()
            });

            return View(adaptersList);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var adapter = _repo.GetById((int)id);

            if (adapter == null) return HttpNotFound();

            //Alternative to populate dropdownlist, see "Edit View"
            //SetAdapterType2();

            var adapterViewModel = new AdapterBasicViewModel()
            {
                Id = adapter.Id,
                AdapterName = adapter.AdapterName,
                AdapterTypeId = adapter.AdapterTypeId,
                IsActive = adapter.IsActive,
                ModifiedDate= adapter.ModifiedDate,
                RegistrationDate = adapter.RegistrationDate,

                AdapterTypeList = GetAdapterTypes()
            };

            return View(adapterViewModel);
        }

        public ActionResult Edit(int? id, bool? saveChangesError = false)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (saveChangesError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Edit failed. Try again, and if the problem persists see your system administrator.";

            var adapter = _repo.GetById((int)id);
            if (adapter == null) return HttpNotFound();

            var adapterViewModel = new AdapterBasicViewModel()
            {
                Id = adapter.Id,
                AdapterName = adapter.AdapterName,
                AdapterTypeId = adapter.AdapterTypeId,
                IsActive = adapter.IsActive,
                ModifiedDate = adapter.ModifiedDate,
                RegistrationDate = adapter.RegistrationDate,

                AdapterTypeList = GetAdapterTypes()
            };

            return View(adapterViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "")] AdapterBasicViewModel adapter)
        {
            #region For keeping note 
            ////1. way 
            //var adapterNameRequest = Request.Form["AdapterName"];
            ////2. way 
            //FormCollection values = null; //TODO : this needs to come as parameter
            //var adapterNameFormCollection = values["AdapterName"]; //Parameter : FormCollection values
            #endregion 

            //TODO : there may be AdapterType with id : 0
            //TODO : How to validate AdapterTypeId ???
            //TODO : How to catch ModelState.Error
            if(adapter.AdapterTypeId <= 0)
                ViewData.ModelState.AddModelError("AdapterTypeId", new ArgumentNullException("AdapterTypeId", "Please select an adapter type."));

            if (ModelState.IsValid)
            {
                using (UoW)
                {
                    _repo.Update(new AdapterBasicDTO()
                    {
                        Id = adapter.Id,
                        AdapterName = adapter.AdapterName,
                        AdapterTypeId = adapter.AdapterTypeId,
                        IsActive = adapter.IsActive,
                        ModifiedDate = adapter.ModifiedDate,
                        RegistrationDate = adapter.RegistrationDate,
                    } );
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit", new { id = adapter.Id, saveChangesError = true });
        }
        

        // GET: AdapterBasic/Create
        public ActionResult Create()
        {
            var adapterViewModel = new AdapterBasicViewModel()
            {
                AdapterTypeList = GetAdapterTypes()
            };

            return View(adapterViewModel);
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "")] AdapterBasicViewModel adapterBasicViewModel)
        {
            if (ModelState.IsValid)
            {
                using (UoW)
                {
                    _repo.Insert(new AdapterBasicDTO()
                    {
                        AdapterName = adapterBasicViewModel.AdapterName,
                        AdapterTypeId = adapterBasicViewModel.AdapterTypeId,
                        Id = adapterBasicViewModel.Id,
                        IsActive = adapterBasicViewModel.IsActive,
                        ModifiedDate = adapterBasicViewModel.ModifiedDate,
                        RegistrationDate = adapterBasicViewModel.RegistrationDate
                    });
                }
                return RedirectToAction("Index");
            }

            var adapterViewModel = new AdapterBasicViewModel()
            {
                AdapterTypeList = GetAdapterTypes()
            };

            return View(adapterViewModel);
        }

        /* 
        
        // GET: AdapterBasic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdapterBasic/Delete/5
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
        */
    }
}
