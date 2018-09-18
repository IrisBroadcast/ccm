using System.Collections.Generic;
using CCM.Core.Entities.Specific;

namespace CCM.Web.Models.Home
{
    public class RegisteredSipUpdateViewModel
    {
        public int Count { get; set; }
        public List<RegisteredSipDto> Data { get; set; }
    }
}