using System;

namespace CCM.Core.Entities.Specific
{
    public class OldCall : OnGoingCall
    {
        public DateTime Ended { get; set; }
        public string Duration { get; set; }
    }
}