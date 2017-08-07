using Shortsite.Models;
using Shortsite.Utils;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace Shortsite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(string id = null, SiteModel model = null)
        {
            if ((model == null) && (id == null))
            {
                SiteModel newmodel = new SiteModel();
                return View(newmodel);
            }

            //É para quando receber um codigo de algum site, redirecionar para esse site
            if (id != null)
            {
                SaveToDB save = new SaveToDB();
                string originalUrl = save.get_originalUrl_from_redis(id);
                save.increment_access_count(originalUrl);
                
                return (!originalUrl.Contains("http")) ? Redirect("http://" + originalUrl) : Redirect(originalUrl);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string originalSite, string alias = "")
        {
            Generate gen = new Generate();
            SiteModel model = new SiteModel();
            Stopwatch time = new Stopwatch();
            alias = alias.ToLower();

            time.Start();
            try
            {
                if ((alias == "create-short-url") || alias == "ranking")
                {
                    throw new InvalidAliasException();
                }
                model.Url = (String.IsNullOrEmpty(alias)) ? gen.shortSite(originalSite) : gen.shortSite(originalSite, alias);
            }
            catch (AliasExistsException e)
            {
                model.Error = e.Message;
            }
            catch (InvalidAliasException e)
            {
                model.Error = e.Message;
            }

            time.Stop();
            model.Time = time.Elapsed.Milliseconds;
            model.Alias = (String.IsNullOrEmpty(alias)) ? model.Url.Split('/')[1] : alias ;

            return RedirectToAction("Index", model);
        }
    }
}