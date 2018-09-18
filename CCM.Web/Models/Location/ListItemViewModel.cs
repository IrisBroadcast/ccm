using System;

namespace CCM.Web.Models.Location
{
    public class ListItemViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int SortIndex { get; set; }
    }
}