using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Models;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Modules.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Modules
{
    public static class Init
    {
        public static void InitializeMigration(MigrateForm migrateForm)
        {
            // Process Root
            // Create JSS Root

            // Process page

            ProcessPageItem.process(migrateForm);

            // Process layout
            // Add layout service place holders

        }
    }
}