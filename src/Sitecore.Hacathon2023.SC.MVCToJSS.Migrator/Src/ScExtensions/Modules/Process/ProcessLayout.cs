using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Helper;
using Sitecore.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Hacathon2023.SC.MVCToJSS.Migrator.Modules.Process
{
    public static class ProcessLayout
    {
        public static bool ProcesssItemAllLayout(Item mvcPageItem)
        {
            bool result = false;

            try
            {
                var renderingCount = SitecoreHelper.CountRenderings(mvcPageItem);

                // Process sharedlayout
                if (renderingCount.Item1 > 0)
                {
                    ProcesssItemLayout(mvcPageItem, Sitecore.FieldIDs.LayoutField);
                }

                // Process Finallayout
                if (renderingCount.Item2 > 0)
                {
                    ProcesssItemLayout(mvcPageItem, Sitecore.FieldIDs.FinalLayoutField);
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public static bool ProcesssItemLayout(Item mvcPageItem, ID currentLayoutId)
        {
            bool result = false;

            try
            {
                var renderingCount = SitecoreHelper.CountRenderings(mvcPageItem);

                // Process sharedlayout
                if (renderingCount.Item1 > 0)
                {
                    ////Grab the field that contains the layout
                    //var layoutField = new LayoutField(mvcPageItem.Fields[Sitecore.FieldIDs.LayoutField]);

                    //Grab the field that contains the final layout
                    var currentLayoutField = new LayoutField(mvcPageItem.Fields[currentLayoutId]);

                    if (currentLayoutField != null)
                    {
                        //If we don't have a final layout delta, we're good!
                        if (!string.IsNullOrWhiteSpace(currentLayoutField.Value))
                        {
                            var currentLayoutDefinition = LayoutDefinition.Parse(currentLayoutField.Value);


                            DeviceDefinition deviceDefinition = currentLayoutDefinition.GetDevice(MVCToJSSConstants.DeviceIdTemplateId);
                            

                            // Handling Layout file
                            string mvcLayoutID = deviceDefinition.Layout;
                            Item mvcLayoutItem = Sitecore.Context.Database.GetItem(mvcLayoutID);
                            var mvcLayoutItemTemplate = TemplateManager.GetTemplate(mvcLayoutItem);
                            // Create layout and map it
                            if (!mvcLayoutItemTemplate.Name.Equals("JSS Layout", StringComparison.OrdinalIgnoreCase))
                            {
                                string jssLayoutName = mvcLayoutItem.Name + "-jss";
                                string layoutItemJSSFullPath = string.Concat(mvcLayoutItem.Parent.Paths.FullPath, "/", jssLayoutName);
                                Item layoutItemJSS = Sitecore.Context.Database.GetItem(layoutItemJSSFullPath);
                                if (layoutItemJSS == null)
                                {
                                    // Create Layout

                                    // Get the template for which you need to create item
                                    TemplateItem layoutJSSTemplate = Sitecore.Context.Database.GetItem(MVCToJSSConstants.JSSLayoutTemplate);

                                    // Get the place in the site tree where the new item must be inserted
                                    Item layoutParentItem = Sitecore.Context.Database.GetItem(mvcLayoutItem.ParentID);
                                    
                                    // Copy MVC item to new JSS item
                                    layoutItemJSS = mvcLayoutItem.CopyTo(layoutParentItem, jssLayoutName);
                                    // Change template                                   
                                    layoutItemJSS.ChangeTemplate(layoutJSSTemplate);

                                    using (new EditContext(layoutItemJSS))
                                    {
                                        // Save the layout changes
                                        layoutItemJSS.Editing.BeginEdit();
                                        // Change path
                                        layoutItemJSS["Path"] = "/Views/JssApp.cshtml";
                                        layoutItemJSS.Editing.AcceptChanges();
                                        layoutItemJSS.Editing.EndEdit();
                                    }                                    

                                    
                                }

                                deviceDefinition.Layout = layoutItemJSS.ID.Guid.ToString();
                            }

                            /// Get the array of all renderings for the target page item                    
                            IEnumerable<RenderingDefinition> renderingsArray = deviceDefinition.Renderings.ToArray().Cast<RenderingDefinition>();
                            int renderingIndex = 0;
                            foreach (RenderingDefinition renderingDefinitionMVC in renderingsArray)
                            {
                                // Get existing rendering 
                                Item renderingItemMVC = Sitecore.Context.Database.GetItem(renderingDefinitionMVC.ItemID);
                                var renderingItemMVCTemplate = TemplateManager.GetTemplate(renderingItemMVC);

                                if (!renderingItemMVCTemplate.Name.Equals("Json Rendering", StringComparison.OrdinalIgnoreCase))
                                {
                                    string jssRenderingName = renderingItemMVC.Name + "-jss";
                                    string renderingItemJSSFullPath = string.Concat(renderingItemMVC.Parent.Paths.FullPath, "/", jssRenderingName);
                                    Item renderingItemJSS = Sitecore.Context.Database.GetItem(renderingItemJSSFullPath);

                                    //Check and create JsonRendering
                                    if (renderingItemJSS == null)
                                    {
                                        // Create JSON rendering

                                        // Get the template for which you need to create item
                                        TemplateItem jsonRenderingTemplate = Sitecore.Context.Database.GetItem(MVCToJSSConstants.JSSRenderingTemplate);

                                        // Get the place in the site tree where the new item must be inserted
                                        Item parentItem = Sitecore.Context.Database.GetItem(renderingItemMVC.ParentID);

                                        //// Add the item to the site tree
                                        //renderingItemJSS = parentItem.Add(jssRenderingName, template);

                                        // Copy MVC item to new JSS item
                                        renderingItemJSS = renderingItemMVC.CopyTo(parentItem, jssRenderingName);
                                        // Change template                                   
                                        renderingItemJSS.ChangeTemplate(jsonRenderingTemplate);

                                        //using (new EditContext(renderingItemJSS))
                                        //{
                                        //    // Save the layout changes
                                        //    renderingItemJSS.Editing.BeginEdit();
                                        //    renderingItemJSS.Name = jssRenderingName;
                                        //    renderingItemJSS.Fields["Component Name"].Value = jssRenderingName;
                                        //    renderingItemJSS.Fields["__Component name"].Value = jssRenderingName;

                                        //    renderingItemJSS["Component Name"] = jssRenderingName;
                                        //    renderingItemJSS["__Component name"] = jssRenderingName;
                                        //    //mvcPageItem.Fields["__Final Renderings"].Reset();
                                        //    renderingItemJSS.Editing.AcceptChanges();
                                        //    renderingItemJSS.Editing.EndEdit();
                                        //}

                                    }


                                    if (renderingItemMVC != null && renderingItemJSS != null)
                                    {
                                        // Remove MVC rendering and
                                        deviceDefinition.Renderings.RemoveAt(renderingIndex);
                                        //deviceDefinition.Renderings = new ArrayList(renderingsArray.Where(r => !string.Equals(r.UniqueId, renderingDefinitionMVC.UniqueId,StringComparison.OrdinalIgnoreCase)).ToList());

                                        // ADD JSS rendering and configure props
                                        RenderingDefinition renderingDefinitionJSS = new RenderingDefinition();
                                        renderingDefinitionJSS.ItemID = renderingItemJSS.ID.Guid.ToString();
                                        renderingDefinitionJSS.Placeholder = renderingDefinitionMVC.Placeholder;
                                        renderingDefinitionJSS.Datasource = renderingDefinitionMVC.Datasource;
                                        renderingDefinitionJSS.Parameters = renderingDefinitionMVC.Parameters;
                                        renderingDefinitionJSS.Rules = renderingDefinitionMVC.Rules;
                                        renderingDefinitionJSS.DynamicProperties = renderingDefinitionMVC.DynamicProperties;
                                        renderingDefinitionJSS.Conditions = renderingDefinitionMVC.Conditions;

                                        deviceDefinition.Insert(renderingIndex, renderingDefinitionJSS);

                                    }

                                    using (new EditContext(mvcPageItem))
                                    {
                                        // Save the layout changes
                                        mvcPageItem.Editing.BeginEdit();
                                        currentLayoutField.Value = currentLayoutDefinition.ToXml();
                                        //mvcPageItem.Fields["__Final Renderings"].Reset();
                                        mvcPageItem.Editing.AcceptChanges();
                                        mvcPageItem.Editing.EndEdit();
                                    }
                                }

                                renderingIndex++;
                            }




                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }


    }
}