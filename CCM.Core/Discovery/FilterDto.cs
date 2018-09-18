using System.Collections.Generic;

namespace CCM.Core.Discovery
{
    public class FilterDto
    {
        public string Name { get; set; }
        public IList<string> Options { get; set; }
    }
}