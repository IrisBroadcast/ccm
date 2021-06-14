﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Models
{
    public class CallHistories
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CallId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public bool IsPhoneCall { get; set; }

        [Column("SipCallId")]
        public string DialogCallId { get; set; }
        [Column("DlgHashId")]
        public string DialogHashId { get; set; }
        [Column("DlgHashEnt")]
        public string DialogHashEnt { get; set; }

        public Guid FromId { get; set; }
        public string FromTag { get; set; }
        public string FromSip { get; set; }
        public string FromUsername { get; set; }
        public string FromDisplayName { get; set; }
        public string FromComment { get; set; }
        public Guid FromLocationId { get; set; }
        public string FromLocationName { get; set; }
        public string FromLocationComment { get; set; }
        public string FromLocationShortName { get; set; }
        [Column("FromLocCat")]
        public string FromLocationCategory { get; set; }
        public Guid FromCodecTypeId { get; set; }
        public string FromCodecTypeName { get; set; }
        public string FromCodecTypeColor { get; set; }
        [Column("FromTypeCat")]
        public string FromCodecTypeCategory { get; set; }
        public Guid FromOwnerId { get; set; }
        public string FromOwnerName { get; set; }
        public Guid FromRegionId { get; set; }
        public string FromRegionName { get; set; }
        [Column("FromUserAgentHead")]
        public string FromUserAgentHeader { get; set; }

        public Guid ToId { get; set; }
        public string ToTag { get; set; }
        public string ToSip { get; set; }
        public string ToUsername { get; set; }
        public string ToDisplayName { get; set; }
        public string ToComment { get; set; }
        public Guid ToLocationId { get; set; }
        public string ToLocationName { get; set; }
        public string ToLocationComment { get; set; }
        public string ToLocationShortName { get; set; }
        [Column("ToLocCat")]
        public string ToLocationCategory { get; set; }
        public Guid ToCodecTypeId { get; set; }
        public string ToCodecTypeName { get; set; }
        public string ToCodecTypeColor { get; set; }
        [Column("ToTypeCat")]
        public string ToCodecTypeCategory { get; set; }
        public Guid ToOwnerId { get; set; }
        public string ToOwnerName { get; set; }
        public Guid ToRegionId { get; set; }
        public string ToRegionName { get; set; }
        [Column("ToUserAgentHead")]
        public string ToUserAgentHeader { get; set; }
    }
}
