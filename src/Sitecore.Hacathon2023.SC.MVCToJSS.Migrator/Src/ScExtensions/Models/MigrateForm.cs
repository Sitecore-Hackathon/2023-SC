using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Models
{
    public class MigrateForm
    {
        public string MVCWebSiteRoot { get; set; }
        public string JSSWebSiteRoot { get; set; }
        public bool CodeGeneration { get; set; }
    }
}