using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("Logs")]
    public class LogEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public int LevelValue { get; set; }
        public string Message { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
        public string Application { get; set; }
        public Guid ActivityId { get; set; }
    }
}