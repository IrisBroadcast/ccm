using System.Net;
using System.Net.Security;
using CCM.Core.Helpers;
using NLog;

namespace CCM.Web.Infrastructure
{
    public static class CertificateHelper
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Pga att certifikatet på discovery-servern inte överensstämmer med URL:en gör vi undantag här och släpper igenom 
        /// anrop till discovery trots felaktigt namn.
        /// </summary>
        public static bool ServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            log.Debug("Certificate validation callback. Sender: {0}, Certification: {1}, Chain: {2}, SSL policyErrors: {3}",
                sender, certification, chain, sslPolicyErrors);

            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            if (sslPolicyErrors != SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                return false;
            }

            var request = sender as HttpWebRequest;
            if (request == null)
            {
                return false;
            }

            if (request.Address.Host != ApplicationSettings.DiscoveryHost.Host)
            {
                return false;
            }

            log.Info("Accepting request to {0} although certificate name mismatch", request.Address);
            return true;
        }
    }
}