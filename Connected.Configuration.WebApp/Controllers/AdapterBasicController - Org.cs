using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Connected.Common;
using Connected.DAL.Core.Configuration.Model;
using Connected.DAL.Core;

namespace Org.Connected.Configuration.WebApp.Controllers
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
            UoW = IoCManager.Instance.Resolve<IUnitOfWork>();
            _repo = UoW.GetRep<AdapterBasicDTO>();
            _adapterTypesRepo = UoW.GetRep<AdapterTypeDIMDTO>();
        }

        private void SetAdapterType()
        {
            var types = _adapterTypesRepo.GetAll();

            ViewBag.AdapterTypes = types;
        }

        private void SetAdapterType2(int? adapterTypeId = null)
        {
            var types = _adapterTypesRepo.GetAll();

            ViewBag.AdapterTypes = adapterTypeId != null ? 
                new SelectList(types, "Id", "AdapterType", adapterTypeId) : 
                new SelectList(types, "Id", "AdapterType");
        }

        // GET: AdapterBasic
        public ActionResult Index()
        {
            IEnumerable<AdapterBasicDTO> adaptersList = _repo.GetAll();

            SetAdapterType();
            return View(adaptersList);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var adapter = _repo.GetById((int)id);

            if (adapter == null) return HttpNotFound();

            //SetAdapterType();
            SetAdapterType2();
            return View(adapter);
        }

        public ActionResult Edit(int? id, bool? saveChangesError = false)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (saveChangesError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Edit failed. Try again, and if the problem persists see your system administrator.";

            var adapter = _repo.GetById((int)id);
            if (adapter == null) return HttpNotFound();

            //Populate adapter types
            SetAdapterType();

            return View(adapter);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind( Include = "")] AdapterBasicDTO adapterBasicDTO)
        {
            #region For keeping note 
            //1. way 
            var adapterNameRequest = Request.Form["AdapterName"];
            //2. way 
            FormCollection values = null;
            var adapterNameFormCollection = values["AdapterName"]; //Parameter : FormCollection values

            #endregion 
            //TODO : there may be AdapterType with id : 0
            //TODO : How to validate AdapterTypeId ???
            //TODO : How to catch ModelState.Error
            if(adapterBasicDTO.AdapterTypeId <= 0)
                ViewData.ModelState.AddModelError("AdapterTypeId", new ArgumentNullException("AdapterTypeId", "Please select an adapter type."));

            if (ModelState.IsValid)
            {
                using (UoW)
                {
                    _repo.Update(adapterBasicDTO);
                }

                return RedirectToAction("Index");
            }

            SetAdapterType();
            return RedirectToAction("Edit", new { id = adapterBasicDTO.Id, saveChangesError = true });
        }
        

        // GET: AdapterBasic/Create
        public ActionResult Create()
        {
            SetAdapterType();
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "")] AdapterBasicDTO adapterBasicDTO)
        {
            if (ModelState.IsValid)
            {
                using (UoW)
                {
                    _repo.Insert(adapterBasicDTO);
                }
                return RedirectToAction("Index");
            }

            SetAdapterType();
            return View(adapterBasicDTO);
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
