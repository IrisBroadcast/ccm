using System;
using CCM.Core.CodecControl.Entities;

namespace CCM.Core.Interfaces.Repositories.Specialized
{
    public interface ICodecInformationRepository
    {
        CodecInformation GetCodecInformationBySipAddress(string sipAddress);
        CodecInformation GetCodecInformationById(Guid id);
    }
}