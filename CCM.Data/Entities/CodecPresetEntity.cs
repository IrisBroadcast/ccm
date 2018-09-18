using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("CodecPresets")]
    public class CodecPresetEntity : EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<UserAgentEntity> UserAgents { get; set; }
    }
}