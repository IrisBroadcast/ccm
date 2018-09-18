using System;
using System.Data.Entity;
using System.Linq;
using CCM.Core.CodecControl.Entities;
using CCM.Core.Interfaces;
using CCM.Core.Interfaces.Repositories.Specialized;
using CCM.Data.Entities;
using LazyCache;

namespace CCM.Data.Repositories.Specialized
{
    public class CodecInformationRepository : BaseRepository, ICodecInformationRepository
    {
        public CodecInformationRepository(IAppCache cache) : base(cache)
        {
        }

        public CodecInformation GetCodecInformationBySipAddress(string sipAddress)
        {
            using (var db = GetDbContext())
            {
                RegisteredSipEntity rs = db.RegisteredSips
                    .Include(o => o.UserAgent)
                    .SingleOrDefault(o => o.SIP == sipAddress);

                return rs == null ? null : MapToCodecInformation(rs);
            }
        }

        public CodecInformation GetCodecInformationById(Guid id)
        {
            using (var db = GetDbContext())
            {
                RegisteredSipEntity rs = db.RegisteredSips
                    .Include(o => o.UserAgent)
                    .SingleOrDefault(o => o.Id == id);

                return rs == null ? null : MapToCodecInformation(rs);
            }
        }

        private CodecInformation MapToCodecInformation(RegisteredSipEntity rs)
        {
            return new CodecInformation
            {
                SipAddress = rs.SIP,
                Api = rs.UserAgent.Api,
                Ip = rs.IP,
                GpoNames = rs.UserAgent.GpoNames,
                NrOfInputs = rs.UserAgent.Inputs
            };
        }
    }
}
