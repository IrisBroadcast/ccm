using System;

namespace CCM.Web.Models.CodecControl
{
    public class SetInputEnabledParameters
    {
        public Guid Id { get; set; }
        public int Input { get; set; }
        public bool Enabled { get; set; }
    }

    public class LoadPresetParameters
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class RebootCodecParameters
    {
        public Guid Id { get; set; }
    }

}