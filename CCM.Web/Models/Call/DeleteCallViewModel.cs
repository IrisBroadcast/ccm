using System;
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.Call
{
    public class DeleteCallViewModel
    {
        public Guid CallId { get; set; }
        public Core.Entities.CallInfo Call { get; set; }

        public string CallFromSipAddress { get; set; }
        public string CallToSipAddress { get; set; }
        public DateTime CallStarted { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Delete_Call_I_Have_Checked")]
        public bool IHaveChecked { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Delete_Call_I_Am_Sure")]
        public bool ImSure { get; set; }
    }
}