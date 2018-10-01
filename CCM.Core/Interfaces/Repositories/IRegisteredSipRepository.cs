using System;
using System.Collections.Generic;
using CCM.Core.CodecControl.Entities;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Kamailio;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IRegisteredSipRepository
    {
        KamailioMessageHandlerResult UpdateRegisteredSip(RegisteredSip registeredSip);
        List<RegisteredSipDto> GetCachedRegisteredSips();
        KamailioMessageHandlerResult DeleteRegisteredSip(string sipAddress);
        List<CodecInformation> GetCodecInformationList();
        CodecInformation GetCodecInformation(string sipAddress);
    }
}
