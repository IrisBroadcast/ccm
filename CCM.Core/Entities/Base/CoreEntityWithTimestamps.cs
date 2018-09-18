using System;

namespace CCM.Core.Entities.Base
{
    public class CoreEntityWithTimestamps : CoreEntityBase
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}