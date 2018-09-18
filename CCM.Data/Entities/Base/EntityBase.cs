using System;
using System.ComponentModel.DataAnnotations;

namespace CCM.Data.Entities.Base
{
    

    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}