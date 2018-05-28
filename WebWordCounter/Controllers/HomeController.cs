using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebWordCounter.Common;
namespace WebWordCounter.Controllers
{
    public class HomeController : Controller
    {
        IBuildWordCloud _buildWordCloud = null;
        public HomeController(IBuildWordCloud buildWordCloud)
        {
            _buildWordCloud = buildWordCloud;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Main()
        {
            return View("Main");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Main(FormCollection formValues)
        {
            var url = formValues["urlPath"];
            if (string.IsNullOrEmpty(url) || !url.StartsWith("http"))
            {
                return new EmptyResult();
            }
            var listWords = _buildWordCloud.GetListTopWords(url);
            if (listWords == null || listWords.Count() == 0)
            {
                return new EmptyResult();
            }
            return PartialView("WordCloud", listWords);
        }

    }
}
