using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCM.Core.Entities;

namespace CCM.Web.Models.Meta
{
    public class MetaFormViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "MetaType_Name")]
        public string MetaTypeName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "MetaType_Value")]
        public string SelectedMetaTypeValue { get; set; }

        public List<AvailableMetaType> MetaTypeValues { get; set; }
    }
}