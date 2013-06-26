using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteTaLoc.Controllers
{
    public class CategoryStorageController : Controller
    {

        NoteTaLoc.Utilitary.CategoryStorage categoryStorage;

        public CategoryStorageController()
        {
            categoryStorage = new Utilitary.CategoryStorage();
        }

        public JsonResult GetCategory(String language)
        {
            return categoryStorage.getCategoryListJSON(language);
        }

        public JsonResult GetCriteria(String language)
        {
            return categoryStorage.getCriteriaListJSON(language);
        }

    }
}
