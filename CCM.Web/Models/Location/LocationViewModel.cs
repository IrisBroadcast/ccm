using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace CCM.Web.Models.Location
{
    public class LocationViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Name_Required")]
        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Net")]
        public string Net { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Cidr")]
        public byte? Cidr { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NetV6")]
        public string NetV6 { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "CidrV6")]
        public byte? CidrV6 { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Carrier_Connection_Id")]
        public string CarrierConnectionId { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "ProfileGroup")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ProfileGroup_Required")]
        public Guid? ProfileGroup { get; set; }

        public List<ListItemViewModel> ProfileGroups { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Region")]
        public Guid Region { get; set; }

        public List<ListItemViewModel> Regions { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "City")]
        public Guid City { get; set; }

        public List<ListItemViewModel> Cities { get; set; }

        [MaxLength(8)]
        [Display(ResourceType = typeof(Resources), Name = "Location_Short_Name")]
        public string ShortName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Comment")]
        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Net))
            {
                if (Cidr == null)
                {
                    yield return new ValidationResult("CIDR för IPv4 saknas", new[] { "Cidr" });
                }
                else
                {
                    IPAddress ipAddress;
                    if (IPAddress.TryParse(Net, out ipAddress))
                    {
                        if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
                        {
                            yield return new ValidationResult("Endast IP v4-adresser kan anges", new[] { "Net" });
                        }
                    }
                    else
                    {
                        yield return new ValidationResult("Ogiltig IPv4-adress", new[] { "Net" });
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(NetV6))
            {
                if (CidrV6 == null)
                {
                    yield return new ValidationResult("CIDR för IPv6 saknas", new[] { "CidrV6" });
                }
                else
                {
                    IPAddress ipAddress;
                    if (IPAddress.TryParse(NetV6, out ipAddress))
                    {
                        if (ipAddress.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            yield return new ValidationResult("Endast IPv6-adresser kan anges", new[] { "NetV6" });
                        }
                    }
                    else
                    {
                        yield return new ValidationResult("Ogiltig IPv6-adress", new[] { "NetV6" });
                    }
                }
            }

        }
    }
}