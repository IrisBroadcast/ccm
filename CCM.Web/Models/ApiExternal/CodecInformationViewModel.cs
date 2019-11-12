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
using System.Runtime.Serialization;

namespace CCM.Web.Models.ApiExternal
{
    [DataContract]
    public class CodecInformationViewModel
    {
        public CodecInformationViewModel(
            string sipAddress,
            string ip,
            string api,
            string userAgent,
            int nrOfInputs,
            int nrOfOutputs,
            int nrOfHeadphones,
            int nrOfGpis,
            int nrOfGpos)
        {
            SipAddress = sipAddress;
            Ip = ip;
            Api = api;
            UserAgent = userAgent; 
            NrOfInputs = nrOfInputs;
            NrOfOutputs = nrOfOutputs;
            NrOfHeadphones = nrOfHeadphones;
            NrOfGpis = nrOfGpis;
            NrOfGpos = nrOfGpos;
        }

        [DataMember]
        public string SipAddress { get; protected set; }

        [DataMember]
        public string Ip { get; protected set; }

        [DataMember]
        public string Api { get; protected set; }

        [DataMember]
        public string UserAgent { get; protected set; }

        //[DataMember]
        //public string GpoNames { get; protected set; } // TODO: Remove this from database columns as well.. 

        [DataMember]
        public int NrOfInputs { get; protected set; }

        [DataMember]
        public int NrOfOutputs { get; protected set; }

        [DataMember]
        public int NrOfHeadphones { get; protected set; }

        [DataMember]
        public int NrOfGpis { get; protected set; }

        [DataMember]
        public int NrOfGpos { get; protected set; }
    }
}
