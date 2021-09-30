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
using System.Collections.Generic;

namespace CCM.Core.Entities
{
    /// <summary>
    /// This one needs to cover FilterParameters that can be used in Discovery.
    /// It does not need to have meta parameter data tags. That is set when
    /// extracting from database. Then filtered at a later stage on what's
    /// actually interesting for the receiver of Discovery service.
    /// </summary>
    public class RegisteredUserAgentDiscovery
    {
        public RegisteredUserAgentDiscovery(
            Guid id,
            DateTime updated,
            string sipUri,
            string displayName,
            string ipAddress,
            string userAgentHeader,
            Guid? userAgentId,
            string userAgentName,
            Guid? locationId,
            string locationName,
            string locationShortName,
            string regionName,
            string cityName,
            string userOwnerName,
            string userDisplayName,
            string codecTypeName,
            List<KeyValuePair<string, string>> metaData
            )
        {
            Id = id;
            Updated = updated;
            SipUri = sipUri;
            DisplayName = displayName;
            IpAddress = ipAddress;
            UserAgentHeader = userAgentHeader;
            UserAgentId = userAgentId;
            UserAgentName = userAgentName;
            LocationId = locationId;
            LocationName = locationName;
            LocationShortName = locationShortName;
            RegionName = regionName;
            CityName = cityName;
            UserOwnerName = userOwnerName;
            UserDisplayName = userDisplayName;
            CodecTypeName = codecTypeName;
            MetaData = metaData;
        }
        // TODO: Should also change the editor for the filtering
        public Guid Id { get; }
        public DateTime Updated { get; }
        public string SipUri { get; }
        public string DisplayName { get; }
        public string IpAddress { get; }
        public string UserAgentHeader { get; }
        public Guid? UserAgentId { get; }
        public string UserAgentName { get; }
        public Guid? LocationId { get; }
        public string LocationName { get; }
        public string LocationShortName { get; }
        public string RegionName { get; }
        public string CityName { get; }
        public string UserOwnerName { get; }
        public string UserDisplayName { get; }
        public string CodecTypeName { get; }
        public List<KeyValuePair<string, string>> MetaData { get; }
    }
}
