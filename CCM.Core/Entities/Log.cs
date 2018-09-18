using System;

namespace CCM.Core.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Callsite { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Application { get; set; }
        public Guid ActivityId { get; set; }
    }
}