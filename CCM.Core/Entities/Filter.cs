using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class Filter : CoreEntityWithTimestamps
    {
        public string Name { get; set; }
        public string ColumnName { get; set; } // Property/column name name in db
        public string TableName { get; set; } // Type (table) in db
        public string FilteringName { get; set; } // Property on cached registered sip object
    }
}