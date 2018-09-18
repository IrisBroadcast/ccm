using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("Filters")]
    public class FilterEntity : EntityBase
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public string FilteringName { get; set; }
    }
}