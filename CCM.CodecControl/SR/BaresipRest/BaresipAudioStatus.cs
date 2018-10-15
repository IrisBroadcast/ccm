/*
 * Copyright (c) 2018 Sveriges Radio AB, Stockholm, Sweden
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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

