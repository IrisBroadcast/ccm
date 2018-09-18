using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("Roles")]
    public class RoleEntity : EntityBase
    {
        public string Name { get; set; }
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}