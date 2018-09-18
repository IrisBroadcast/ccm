using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("LocalPasswords")]
    public class LocalPasswordEntity
    {
        [Key] 
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}