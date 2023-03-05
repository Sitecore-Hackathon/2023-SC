using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Helper
{
    public static class SitecoreHelper
    {
        /// <summary>
        /// Checks the Item inherited from the templates
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="templateIdentifier"></param>
        /// <returns></returns>
        public static bool IsInheritsTemplate(Item candidate, string templateIdentifier)
        {
            bool result = false;
            result = (candidate != null || !string.IsNullOrEmpty(templateIdentifier));
            if (result)
            {
                var t = TemplateManager.GetTemplate(candidate);
                var y = TemplateManager.GetTemplate(templateIdentifier, candidate.Database);

                result = (y != null);
                if (result)
                {
                    result = t.InheritsFrom(y.ID);
                }
            }
            return result;
        }

        /// <summary>
        /// DoesItemHasLayout
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static bool DoesItemHasLayout(Item item)
        {
            if (item != null)
            {
                LayoutItem layoutItem = item.Visualization.GetLayout(Sitecore.Context.Device);
                if (layoutItem != null)
                {
                    return true;
                }
            }

            return false;
        }


        // Returns the number of shared and final renderings for an item.
        // Final renderings are based on language/version of the item.
        // This presumes there is only one device with renderings configured.
        public static System.Tuple<int, int> CountRenderings(Sitecore.Data.Items.Item item)
        {
            var sharedRenderingsCount = CountRenderings(item, Sitecore.FieldIDs.LayoutField);
            var finalRenderingCount = CountRenderings(item, Sitecore.FieldIDs.FinalLayoutField);
            return Tuple.Create(sharedRenderingsCount, finalRenderingCount);
        }
        private static int CountRenderings(Sitecore.Data.Items.Item item, Sitecore.Data.ID renderingFieldId)
        {
            var field = item.Fields[renderingFieldId];
            var layoutXml = Sitecore.Data.Fields.LayoutField.GetFieldValue(field);
            var layout = Sitecore.Layouts.LayoutDefinition.Parse(layoutXml);
            var deviceLayout = layout.Devices[0] as Sitecore.Layouts.DeviceDefinition;
            return (deviceLayout?.Renderings.Count) ?? 0;
        }
    }
}