using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("Settings")]
    public class SettingEntity : EntityBase
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}