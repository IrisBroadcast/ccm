using System;

namespace CCM.Web.Models.UserAgents
{
    public class ProfileListItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int SortIndex { get; set; }
    }
}