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

using System.Net;
using System.Net.Security;
using CCM.Core.Helpers;
using NLog;

namespace CCM.Web.Infrastructure
{
    // TODO: Might be implemented again if ccm and discovery have differenc HTTPS certificates
    //public static class CertificateHelper
    //{
    //    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    //    /// <summary>
    //    /// Because the certificate on the discovery-server dont correspond with the URL.
    //    /// we make an exception here and let the request through to discovery, even
    //    /// though the name is wrong
    //    /// </summary>
    //    public static bool ServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification,
    //        System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //    {
    //        log.Debug("Certificate validation callback. Sender: {0}, Certification: {1}, Chain: {2}, SSL policyErrors: {3}",
    //            sender, certification, chain, sslPolicyErrors);

    //        if (sslPolicyErrors == SslPolicyErrors.None)
    //        {
    //            return true;
    //        }

    //        if (sslPolicyErrors != SslPolicyErrors.RemoteCertificateNameMismatch)
    //        {
    //            return false;
    //        }

    //        var request = sender as HttpWebRequest;
    //        if (request == null)
    //        {
    //            return false;
    //        }

    //        if (request.Address.Host != ApplicationSettingsWeb.DiscoveryHost.Host)
    //        {
    //            return false;
    //        }

    //        log.Debug("Accepting request to {0} although certificate name mismatch", request.Address);
    //        return true;
    //    }
    //}
}
