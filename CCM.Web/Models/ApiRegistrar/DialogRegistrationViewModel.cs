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
using System.Text.Json.Serialization;

namespace CCM.Web.Models.ApiRegistrar
{
    public class DialogRegistrationViewModel
    {
        [JsonPropertyName("callId")] public string CallId { get; set; }
        [JsonPropertyName("callHashId")] public string CallHashId { get; set; }
        [JsonPropertyName("callHashEnt")] public string CallHashEnt { get; set; }

        [JsonPropertyName("started")] public DateTime? Started { get; set; } = null;
        [JsonPropertyName("ended")] public DateTime? Ended { get; set; } = null;
        [JsonPropertyName("isPhoneCall")] public bool IsPhoneCall { get; set; }
        [JsonPropertyName("sdp")] public string SDP { get; set; }

        [JsonPropertyName("fromId")] public string FromId { get; set; }
        [JsonPropertyName("fromUsername")] public string FromUsername { get; set; }
        [JsonPropertyName("fromDisplayname")] public string FromDisplayName { get; set; }
        [JsonPropertyName("fromIP")] public string FromIPAddress { get; set; }
        [JsonPropertyName("fromCategory")] public string FromCategory { get; set; }

        [JsonPropertyName("toId")] public string ToId { get; set; }
        [JsonPropertyName("toUsername")] public string ToUsername { get; set; }
        [JsonPropertyName("toDisplayname")] public string ToDisplayName { get; set; }
        [JsonPropertyName("toIP")] public string ToIPAddress { get; set; }
        [JsonPropertyName("toCategory")] public string ToCategory { get; set; }
    }
}
