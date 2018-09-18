using System.Collections.Generic;

namespace CCM.Web.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<CodecTypeViewModel> CodecTypes { get; set; }
        public IEnumerable<string> Regions { get; set; }
    }
}