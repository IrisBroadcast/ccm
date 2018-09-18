using System;

namespace CCM.Core.Entities
{
    public class CallInfo
    {
        public Guid Id { get; set; }
        public string FromSipAddress { get; set; }
        public string ToSipAddress { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public DateTime Started { get; set; }
        public bool Closed { get; set; }
    }
}