using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using CCM.Core.Entities;

namespace CCM.Web.Models.Home
{
    public class RegisteredSipInfoViewModel
    {
        public Guid Id { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool CodecControl { get; set; } // Have Codec Control

        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Status")]
        public bool InCall { get; set; } // Is In Call

        [Display(ResourceType = typeof(Resources), Name = "In_Call_With")]
        public string InCallWithName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Location")]
        public string LocationName { get; set; }

        public string LocationComment { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Region")]
        public string RegionName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "City")]
        public string CityName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Ip_Address")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserAgent")]
        public string UserAgentHeader { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "SIP")]
        public string Sip { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Web_Interface")]
        public string UserInterfaceLink { get; set; }

        public bool UserInterfaceIsOpen { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Use_Scrollbars")]
        public bool UseScrollbars { get; set; }

        public string FinalUserInterfaceLink
        {
            get
            {
                if (String.IsNullOrWhiteSpace(UserInterfaceLink))
                    return String.Empty;
                if (!UserInterfaceIsOpen && !IsAuthenticated)
                    return String.Empty;
                string ip = Ip;
                IPAddress address;
                if (IPAddress.TryParse(Ip, out address) && address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ip = string.Format("[{0}]", address);
                }
                return UserInterfaceLink.Replace("[host]", ip);
            }
        }

        public bool ActiveX { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Image { get; set; }
        public int Inputs { get; set; }
        public int InputMinDb { get; set; }
        public int InputMaxDb { get; set; }
        public int InputGainStep { get; set; }
        public int Lines { get; set; }
        public List<CodecPreset> CodecPresets { get; set; }
    }
}