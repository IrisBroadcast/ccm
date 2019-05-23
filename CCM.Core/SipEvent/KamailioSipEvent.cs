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

using System;
using CCM.Core.SipEvent.Messages;
using Newtonsoft.Json;

namespace CCM.Core.SipEvent
{
    public class KamailioSipEvent
    {
        // Topic: Registration
        [JsonProperty(PropertyName = "event")] public SipEventType Event { get; set; } // "register"
        [JsonProperty(PropertyName = "timestamp")] public long TimeStamp { get; set; }
        [JsonProperty(PropertyName = "registrar")] public string Registrar { get; set; } // "sipregistrar.sr.se"
        [JsonProperty(PropertyName = "regtype")] public string RegType { get; set; } // "rereg"
        [JsonProperty(PropertyName = "expires")] public int Expires { get; set; }
        [JsonProperty(PropertyName = "method")] public string Method { get; set; } // "REGISTER"
        [JsonProperty(PropertyName = "from_uri")] public string FromUri { get; set; } // "sip:test1249@contrib.sr.se" Json-parameter heter from_uri i dialog-meddelande
        [JsonProperty(PropertyName = "from_displayname")] public string FromDisplayName { get; set; } // "Karlstad 11"
        [JsonProperty(PropertyName = "to_uri")] public string ToUri { get; set; } // "<null>"
        [JsonProperty(PropertyName = "to_displayname")] public string ToDisplayName { get; set; } // "<null>"
        [JsonProperty(PropertyName = "auth_user")] public string AuthUser { get; set; } // "test1249"
        [JsonProperty(PropertyName = "user_agent")] public string UserAgentHeader { get; set; } // "Asterisk PBX 11.13"
        [JsonProperty(PropertyName = "requesturi")] public string RequestUri { get; set; } // "sip:contrib.sr.se" Json-parameter heter request_uri i dialog-meddelande
        [JsonProperty(PropertyName = "contact_uri")] public string ContactUri { get; set; } // "<sip:1249@192.121.194.213:5080>"
        [JsonProperty(PropertyName = "call_id")] public string CallId { get; set; } // "338a@contrib.sr.se"

        // Topic: Dialog start
        [JsonProperty(PropertyName = "sipserver")] public string SipServer { get; set; } // "sipregistrar.sr.se"
        [JsonProperty(PropertyName = "dialog_state")] public string DialogState { get; set; } // "start"
        [JsonProperty(PropertyName = "dhash_id")] public string DialogHashId { get; set; } // "10300"
        [JsonProperty(PropertyName = "dhash_ent")] public string DialogHashEntry { get; set; } // "2697"
        [JsonProperty(PropertyName = "pool_uri")] public string PoolUri { get; set; } // "sip:studio-pool-004@contrib.sr.se"
        [JsonProperty(PropertyName = "to_tag")] public string ToTag { get; set; } // "<null>"
        [JsonProperty(PropertyName = "from_tag")] public string FromTag { get; set; } // "svE1Cx0ksdjnQ-Csidg60B6H02bNXbfQ"
        [JsonProperty(PropertyName = "mediastatus")] public string MediaStatus { get; set; } // ""
        [JsonProperty(PropertyName = "sdp")] public string Sdp { get; set; } // "v=0....."
        [JsonProperty(PropertyName = "debug_origuri")] public string DebugOrigUri { get; set; } // "<null>"

        // Topic: Dialog End
        [JsonProperty(PropertyName = "hangup_reason")] public string HangupReason { get; set; } // "NORMAL"

        [JsonProperty(PropertyName = "ip")] public IpInfo Ip { get; set; }

        public string ToLogString()
        {
            var timestamp = this.UnixTimeStampToDateTime(this.TimeStamp);
            return $"Kamailio Sip Event:{this.Event.ToString()}, TimeStamp:{timestamp}, Registrar:{this.Registrar}, RegType:{this.RegType}, Expires:{this.Expires.ToString()}, Method:{this.Method}, FromURI:{this.FromUri}, CallId:{this.CallId.ToString()}" +
            	$", SipServer:{this.SipServer}, DialogState:{this.DialogState}, DialogHashId:{this.DialogHashId}, DialogHashEntry:{this.DialogHashEntry}, HangupReason:{this.HangupReason}";
        }

        public string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // TODO: Maybe move this to a helper function
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime.ToString();
        }
    }

    public class IpInfo
    {
        [JsonProperty(PropertyName = "sender_ip")] public string SenderIp { get; set; } // "192.121.194.213"
        [JsonProperty(PropertyName = "sender_port")] public int SenderPort { get; set; } // "5080"
        [JsonProperty(PropertyName = "our_ip")] public string OurIp { get; set; } // 192.121.194.200
        [JsonProperty(PropertyName = "our_port")] public int OurPort { get; set; } // "5060"
    }

}