using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CCM.Core.Helpers;

namespace CCM.Web.Extensions
{
    public class EnumHelpers
    {
        public static List<SelectListItem> EnumSelectList<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T))
                .OfType<Enum>()
                .Select(a => new SelectListItem() { Text = a.Description(), Value = a.ToString() })
                .ToList();
        }
    }
}