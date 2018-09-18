using System;

namespace CCM.Web.Models.Common
{
    public class DeleteViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }
        public string WarningText { get; set; }
    }
}