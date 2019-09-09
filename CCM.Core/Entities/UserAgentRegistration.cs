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

namespace CCM.Core.Entities
{
    public class UserAgentRegistration
    {
        public UserAgentRegistration(
            string sipUri,
            string userAgentHeader,
            string username,
            string displayName,
            string registrar,
            string ipAddress,
            int port,
            int expirationTimeSeconds,
            long serverTimeStamp
            )
        {
            SipUri = sipUri;
            UserAgentHeader = userAgentHeader;
            Username = username;
            DisplayName = displayName;
            Registrar = registrar;
            IpAddress = ipAddress;
            Port = port;
            ExpirationTimeSeconds = expirationTimeSeconds;
            ServerTimeStamp = serverTimeStamp;
        }

        public string SipUri { get; }
        public string UserAgentHeader { get; }
        public string Username { get; }
        public string DisplayName { get; }
        public string Registrar { get; }
        public string IpAddress { get; }
        public int Port { get; }
        public int ExpirationTimeSeconds { get; }
        public long ServerTimeStamp { get; }
    }
}
