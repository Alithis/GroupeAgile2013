using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteTaLoc.Utilitary;

namespace NoteTaLoc.Controllers
{
    public class CategoryStorageController : Controller
    {

        NoteTaLoc.Utilitary.CategoryStorage categoryStorage;

        public CategoryStorageController()
        {
            categoryStorage = (NoteTaLoc.Utilitary.CategoryStorage) System.Web.HttpContext.Current.Application["Categories"];
        }

        public JsonResult GetCategory(String language)
        {
            JsonResult result = new JsonResult();
            result.Data = categoryStorage.getCategoryListJSON(language);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetCriteria(String language)
        {
            JsonResult result = new JsonResult();
            result.Data = categoryStorage.getCriteriaListJSON(language);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

    }
}
