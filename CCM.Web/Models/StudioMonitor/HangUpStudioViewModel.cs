using System;
using System.ComponentModel.DataAnnotations;

namespace CCM.Web.Models.StudioMonitor
{
    public class HangUpStudioViewModel
    {
        [Display(ResourceType = typeof(Resources), Name = "HangUp_I_Have_Booked")]
        public bool IHaveBooked { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "HangUp_I_Have_Checked")]
        public bool IHaveChecked { get; set; }
        public Guid StudioId { get; set; }
    }
}