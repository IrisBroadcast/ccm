using System.ComponentModel.DataAnnotations.Schema;
using CCM.Data.Entities.Base;

namespace CCM.Data.Entities
{
    [Table("CcmUsers")]
    public class UserEntity : EntityBase
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public virtual RoleEntity Role { get; set; }
        
    }
}