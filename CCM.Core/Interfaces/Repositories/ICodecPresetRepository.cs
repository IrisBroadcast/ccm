using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface ICodecPresetRepository : IRepository<CodecPreset>
    {
        List<CodecPreset> Find(string search);
    }
}
