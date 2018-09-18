using System;
using CCM.Core.Helpers;

namespace CCM.Web.Extensions
{
    public class EnumDto
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static EnumDto Create(Enum e)
        {
            return new EnumDto
            {
                Value = Convert.ToInt32(e),
                Name = e.ToString(),
                Description = e.Description()
            };
        }

    }
}