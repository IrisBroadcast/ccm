using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace CCM.WebCommon.Authentication
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request, HttpStatusCode statusCode = HttpStatusCode.Forbidden)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage(StatusCode)
            {
                RequestMessage = Request,
                ReasonPhrase = ReasonPhrase
            };
        }
    }
}