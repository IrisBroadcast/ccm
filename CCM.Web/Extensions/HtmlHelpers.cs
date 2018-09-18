using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace CCM.Web.Extensions
{
    public static class HtmlHelpers
    {
        /// Code started from SO: 
        /// http://stackoverflow.com/questions/6578495/how-do-i-display-the-displayattribute-description-attribute-value
        /// <summary>
        /// Usage:
        /// @Html.DisplayFor(m => m.PropertyName)
        /// 
        /// supply cssclass name, and override span with div tag
        /// @Html.DisplayFor(m => m.PropertyName, "desc", "div")
        /// 
        /// using the named param
        /// @Html.DisplayFor(m => m.PropertyName, tagName: "div")
        /// </summary>
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string cssClassName = "", string tagName = "span")
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            if (!string.IsNullOrEmpty(description))
            {
                var tag = new TagBuilder(tagName) { InnerHtml = description };
                if (!string.IsNullOrEmpty(cssClassName))
                {
                    tag.AddCssClass(cssClassName);
                }

                return new MvcHtmlString(tag.ToString());
            }

            return MvcHtmlString.Empty;
        }
    }
}
