using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCM.Data.Entities
{
    [Table("CodecPresetUserAgents")]
    public class CodecPresetUserAgent
    {
        [Key, ForeignKey("CodecPreset"), Column(Order = 0)]
        public Guid CodecPresetId { get; set; }

        [Key, ForeignKey("UserAgent"), Column(Order = 1)]
        public Guid UserAgentId { get; set; }

    }
}