using CCM.Core.Entities.Base;

namespace CCM.Core.Entities
{
    public class Setting : CoreEntityWithTimestamps
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}