using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Helper;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Models;
using Sitecore.Layouts;
using Sitecore.SecurityModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Modules.Process
{
    public static class ProcessPageItem
    {
        public static List<Item> ProcessedRenderings = new List<Item>();

        public static void process(MigrateForm migrateForm)
        {

            using (new SecurityDisabler())
            {
                string jssRouteTemplateId = Settings.GetSitecoreSettings(MVCToJSSConstants.JssRouteTemplateId);


                string mvcRootHomeItem = migrateForm.MVCWebSiteRoot;

                Func<string, bool> changeMVCToJSSItem = null;
                changeMVCToJSSItem = (mvcPageItemPath) =>
                 {
                     bool result = false;
                     try
                     {
                         var mvcPageItem = Sitecore.Context.Database.GetItem(mvcPageItemPath);
                         
                         // Check item contains presentation
                         if (SitecoreHelper.DoesItemHasLayout(mvcPageItem))
                         {
                             // Inherit JSS route template to a page
                             if (!SitecoreHelper.IsInheritsTemplate(mvcPageItem, jssRouteTemplateId))
                             {
                                 var mvcPageTemplate = TemplateManager.GetTemplate(mvcPageItem);
                                 var mvcPageTemplateItem = Sitecore.Context.Database.GetItem(mvcPageTemplate.ID);

                                 mvcPageTemplateItem.Editing.BeginEdit();
                                 mvcPageTemplateItem[MVCToJSSConstants.BaseTemplateFieldName] = string.Concat(mvcPageTemplateItem[MVCToJSSConstants.BaseTemplateFieldName], MVCToJSSConstants.PipelineString, jssRouteTemplateId);
                                 mvcPageTemplateItem.Editing.EndEdit();
                             }

                             // Process layout
                             var resultRendering = ProcessLayout.ProcesssItemAllLayout(mvcPageItem);
                         }

                         // Progress items recursively
                         if (mvcPageItem.Children.Any())
                         {
                             foreach (Item childItem in mvcPageItem.Children)
                             {
                                 changeMVCToJSSItem(childItem.Paths.FullPath);
                             }
                         }
                     }
                     catch (Exception ex)
                     {

                     }
                     return result;
                 };

                changeMVCToJSSItem(migrateForm.MVCWebSiteRoot);

                

                // Processing all oof thier children in nested mode
            }
        }
    }
}