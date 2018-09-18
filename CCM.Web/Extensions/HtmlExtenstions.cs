using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CCM.Web.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Changes new line to br tag.
        /// </summary>
        public static MvcHtmlString Nl2Br(this HtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return MvcHtmlString.Create(text);
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                string[] lines = text.Split('\n');

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                    {
                        builder.Append("<br/>");
                    }

                    builder.Append(HttpUtility.HtmlEncode(lines[i]));
                }

                return MvcHtmlString.Create(builder.ToString());
            }
        }
    }
}