using System.Threading.Tasks;
using System.Web.Http;
using CCM.CodecControl.SR.BaresipRest;
using CCM.Core.CodecControl.Entities;
using CCM.Core.CodecControl.Interfaces;

namespace CCM.Web.Controllers.Api
{
    public class TestController : ApiController
    {
        private readonly ICodecManager _codecManager;

        public TestController(ICodecManager codecManager)
        {
            _codecManager = codecManager;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetBaresipAudioStatus()
        {
            var ip = "134.25.127.231";
            var codecInformation = new CodecInformation
            {
                Ip = ip,
                Api = "BaresipRest"
            };

            var api = new BaresipRestApi();
            //var audioStatus = await api.GetAudioStatusAsync(ip);

            var audioStatus = await Task.Run( () => _codecManager.GetAudioStatusAsync(codecInformation, 0, 0));

            return Ok(audioStatus);
        }

    }
}
