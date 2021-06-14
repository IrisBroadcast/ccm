using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class RegisteredCodec
    {
        public Guid Id { get; set; }

        public string SIP { get; set; }

        public string UserAgentHeader { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Registrar { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public long ServerTimeStamp { get; set; }
        public DateTime Updated { get; set; }
        public int Expires { get; set; }

        public Guid? Location_LocationId { get; set; }
        public virtual Location Location { get; set; }

        public Guid? User_UserId { get; set; }
        // TODO: Would probably work with [Key, ForeignKey("User_UserId"), Column("User_UserId"]
        public virtual SipAccount User { get; set; }

        public Guid? UserAgentId { get; set; }
        public virtual UserAgent UserAgent { get; set; }

    }
}
