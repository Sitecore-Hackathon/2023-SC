using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using Sitecore.Xml.Xsl;

namespace Kizad.Foundation.ScExtensions.Extensions
{
    public class RenderPhoneLink
    {
        public void Process(Sitecore.Pipelines.RenderField.RenderFieldArgs args)
        {
            if (args != null && (args.FieldTypeKey == "link" || args.FieldTypeKey == "general link"))
            {
                Sitecore.Data.Fields.LinkField linkField = args.Item.Fields[args.FieldName];
                if (!string.IsNullOrEmpty(linkField.Url) && linkField.LinkType == "tel")
                {
                    args.Parameters["href"] = linkField.Url;
                }
            }
        }

    }
}