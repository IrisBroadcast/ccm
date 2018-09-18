using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCM.Core.Entities;

namespace CCM.Web.Models.Filter
{
    public class FilterFormViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Filter_Name")]
        public string FilterName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Filter_On")]
        public string SelectedFilter { get; set; }

        public List<AvailableFilter> Filters { get; set; }
    }
}