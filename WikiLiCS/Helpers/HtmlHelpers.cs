using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Text;

namespace WikiLiCS.Helpers
{
    public static class HtmlHelpers
    {
        public static string Truncate(this HtmlHelper helper, string input, int length)
        {
            //string n = null;
            //bool b = (n.Equals(input));
            if (!(input == null))
            {
                if (input.Length <= length)
                {
                    return input;
                }
                else
                {
                    return input.Substring(0, length) + "...";
                }
            }
            else
            {
                return input;
            }
        }


        public static MvcHtmlString Menu(this HtmlHelper helper)
        {
            var sb = new StringBuilder();
            sb.Append("<ul class='menu'>");

            var topLevelNodes = SiteMap.RootNode.ChildNodes;
            foreach (SiteMapNode node in topLevelNodes)
            {
                if (SiteMap.CurrentNode == node)
                    sb.AppendLine("<li class='selectedMenuItem'>");
                else
                    sb.AppendLine("<li>");

                sb.AppendFormat("<a href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));
                sb.AppendLine("</li>");
            }

            // Close unordered list tag

            sb.Append("</ul>");

            var sb1 = new MvcHtmlString(sb.ToString());
            return sb1;
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", altText);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}