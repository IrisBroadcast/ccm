using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class MetaType : CoreEntityWithTimestamps
    {
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string Type { get; set; }
        public string FullPropertyName { get; set; }
       
    }
}