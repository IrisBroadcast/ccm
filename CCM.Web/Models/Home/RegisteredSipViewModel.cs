using System;
namespace CCM.Web.Models.Home
{
    public class RegisteredSipViewModel
    {
        public Guid Id { get; set; }
        public string Sip { get; set; }
        public string DisplayName { get; set; }
        public bool InCall { get; set; }
        public string InCallWith { get; set; }
        public string InCallWithSip { get; set; }
        public Guid InCallWithId { get; set; }
        public Core.Entities.Location Location { get; set; }
        public string Channel { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public string UserInterfaceLink { get; set; }
        public string IpAddress { get; set; }
    }
}