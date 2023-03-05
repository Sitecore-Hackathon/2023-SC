using Sitecore.Diagnostics;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Helper;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Models;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Modules;
using Sitecore.SecurityModel;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace SitecoreBugger.Site.Controllers
{
    /// <summary>
    /// DashBoardController
    /// </summary>
    public class DashBoardController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View("~/Views/Modules/MVCToJSS.Migrator/DashBoard/Index.cshtml");
        }

        public string GetStatus()
        {
            return ProgressUpdate.GetCurrentStatus();
        }

        /// <summary>
        /// Migrate
        /// </summary>
        /// <param name="migrateForm"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult Migrate([FromBody] MigrateForm migrateForm)
        {
            try
            {
                ProgressUpdate.IsCurrentlyProgressed = true;

                Init.InitializeMigration(migrateForm);
              
            }
            catch (Exception ex)
            {
                ProgressUpdate.IsCurrentlyProgressed = false;
                var reflectType = System.Reflection.MethodBase.GetCurrentMethod();
                Log.Error(MVCToJSSConstants.ErrorPrefix + reflectType.ReflectedType.Name + reflectType.Name, ex, reflectType);
            }

            ProgressUpdate.IsCurrentlyProgressed = false;

            return null;
        }
    }
}