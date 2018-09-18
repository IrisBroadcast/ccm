using System.Collections.Generic;

namespace CCM.Core.Entities
{
    public class AvailableFilter
    {
        public string Name { get; set; }
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string FilteringName { get; set; }
        public List<string> Options { get; set; }
    }
}