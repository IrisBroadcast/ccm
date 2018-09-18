using System;
using System.Collections.Generic;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories.Base;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IProfileRepository : IRepository<Profile>
    {
        List<Profile> FindProfiles(string searchString);
        IList<ProfileNameAndSdp> GetAllProfileNamesAndSdp();
        IList<ProfileInfo> GetAllProfileInfos();
        void SetProfileSortIndex(IList<Tuple<Guid, int>> profileTuples);
    }
}