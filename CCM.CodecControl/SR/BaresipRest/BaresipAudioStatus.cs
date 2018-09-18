using System.Collections.Generic;

namespace CCM.CodecControl.SR.BaresipRest
{
    public class BaresipLineStatus
    {
        public BaresipState State { get; set; }
        public BaresipCall Call { get; set; }
    }

    public enum BaresipState
    {
        Idle,
        Calling,
        ReceivingCall,
        ConnectedCalled,
        ConnectedReceived,
        Disconnecting
    }

    public class BaresipCall
    {
        public int Code { get; set; } // Status code for last call. Usually a SIP Response code, but 0 when last call terminated normally.
        public string Message { get; set; } // Status message for last call.
        public string ConnectedTo { get; set; }
    }

    public class BaresipAudioStatus
    {
        public Meters Meters { get; set; }
        public List<Input> Inputs { get; set; }
        public List<Output> Outputs { get; set; }
        public Control Control { get; set; }
    }

    public class Meters
    {
        public Levels Tx { get; set; }
        public Levels Rx { get; set; }
    }

    public class Levels
    {
        public int L { get; set; }
        public int R { get; set; }
    }

    public class Input
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool On { get; set; }
        public int Level { get; set; }
        public bool Phantom { get; set; }
        public int State { get; set; }
    }

    public class Output
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool On { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
    }

    public class Control
    {
        public List<Gpo> Gpo { get; set; }
    }

    public class Gpo
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }
}

