using System.Collections.Generic;
using CCM.Core.Attributes;
using CCM.Core.Interfaces;
using CCM.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("Locations")]
    public class LocationEntity : EntityBase, ISipFilter
    {
        [MetaProperty]
        public string Name { get; set; }

        [MetaProperty]
        public string ShortName { get; set; }

        public string Comment { get; set; }

        // IP v4
        public string Net_Address_v4 { get; set; }
        public byte? Cidr { get; set; } // CIDR = Classless Inter-Domain Routing. Bestämmer storleken på nätet

        // IP v6
        public string Net_Address_v6 { get; set; }
        public byte? Cidr_v6 { get; set; }

        [MetaProperty]
        public string CarrierConnectionId { get; set; }

        [MetaType]
        public virtual CityEntity City { get; set; }

        public virtual ICollection<RegisteredSipEntity> RegisteredSips { get; set; }

        [MetaType]
        public virtual RegionEntity Region { get; set; }

        [MetaType]
        public virtual ProfileGroupEntity ProfileGroup { get; set; }

    }
}