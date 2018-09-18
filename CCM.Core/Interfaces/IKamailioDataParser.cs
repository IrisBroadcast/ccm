using CCM.Core.Kamailio.Parser;

namespace CCM.Core.Interfaces
{
    public interface IKamailioDataParser
    {
        KamailioData ParseToKamailioData(string message);
    }
}