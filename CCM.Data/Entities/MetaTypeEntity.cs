using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("MetaTypes")]
    public class MetaTypeEntity : EntityBase
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public string FullPropertyName { get; set; }
    }
}