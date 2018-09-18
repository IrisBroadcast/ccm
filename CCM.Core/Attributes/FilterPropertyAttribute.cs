using System;

namespace CCM.Core.Attributes
{
    public class FilterPropertyAttribute : Attribute
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}