using System;

namespace CCM.Web.Models.Home
{
    public class RegisteredSipOverviewDto
    {
        public bool InCall { get; set; }
        public string Sip { get; set; }
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Location { get; set; }
        public string LocationShortName { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public string CodecTypeName { get; set; }
        public string CodecTypeColor { get; set; }
        public string UserName { get; set; }
        public string UserDisplayName { get; set; }
        public string UserComment { get; set; }
        public string InCallWithId { get; set; }
        public string InCallWithSip { get; set; }
        public string InCallWithName { get; set; }
        public string RegionName { get; set; }
        public DateTime Updated { get; set; }
    }
}