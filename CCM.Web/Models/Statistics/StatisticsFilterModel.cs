using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCM.Core.Entities;

namespace CCM.Web.Models.Statistics
{
    public class StatisticsFilterModel
    {
        [Display(ResourceType = typeof(Resources), Name = "Codec_Type")]
        public List<CodecType> CodecTypes { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Owner")]
        public List<Owner> Owners { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Region")]
        public List<Region> Regions { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "SIP")]
        public List<Core.Entities.SipAccount> Users { get; set; }
    }
}