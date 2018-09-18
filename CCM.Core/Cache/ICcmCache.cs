using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;

namespace CCM.Core.Cache
{
    public interface ICcmCache
    {
        IList<RegisteredSipDto> GetRegisteredSips();
        void ClearRegisteredSips();

        IList<Call> GetCalls();
        void ClearCalls();

        IList<Setting> GetSettings();
        void ClearSettings();

    }
}