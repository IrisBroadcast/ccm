using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CCM.Web.Models.Log
{
    public class LogViewModel
    {
        public string Application { get; set; }
        public int Rows { get; set; }
        public string Search { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public IEnumerable<SelectListItem> Levels { get; set; }
        public int SelectedLevel { get; set; }
        public IEnumerable<SelectListItem> LastOptions { get; set; }
        public string SelectedLastOption { get; set; }
        public Guid ActivityId{ get; set; }
        public List<Core.Entities.Log> LogRows { get; set; }
    }
}