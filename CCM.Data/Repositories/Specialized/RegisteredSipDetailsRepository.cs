using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CCM.Core.Entities;
using CCM.Core.Entities.Specific;
using CCM.Core.Interfaces.Repositories.Specialized;
using LazyCache;

namespace CCM.Data.Repositories.Specialized
{
    public class RegisteredSipDetailsRepository : BaseRepository, IRegisteredSipDetailsRepository
    {
        public RegisteredSipDetailsRepository(IAppCache cache) : base(cache)
        {
        }

        public RegisteredSipDetails GetRegisteredSipById(Guid id)
        {
            using (var db = GetDbContext())
            {
                Entities.RegisteredSipEntity dbSip = db.RegisteredSips
                    .Include(rs => rs.Location)
                    .Include(rs => rs.Location.City)
                    .Include(rs => rs.Location.Region)
                    .Include(rs => rs.UserAgent)
                    .Include(rs => rs.User)
                    .SingleOrDefault(r => r.Id == id);

                return MapToRegisteredSipDetails(dbSip);
            }
        }

        private RegisteredSipDetails MapToRegisteredSipDetails(Entities.RegisteredSipEntity rs)
        {
            if (rs == null)
            {
                return null;
            }

            var model = new RegisteredSipDetails
            {
                Id = rs.Id,
                Sip = rs.SIP,
                DisplayName = rs.DisplayName,
                Ip = rs.IP,
                UserAgentHeader = rs.UserAgentHead,

                Comment = rs.User != null ? rs.User.Comment : string.Empty,
                UserDisplayName = rs.User != null ? rs.User.DisplayName : string.Empty,
                Api = rs.UserAgent != null ? rs.UserAgent.Api ?? string.Empty : string.Empty,
                Image = rs.UserAgent?.Image ?? string.Empty,
                ActiveX = rs.UserAgent != null && rs.UserAgent.Ax,
                Width = rs.UserAgent?.Width ?? 1000,
                Height = rs.UserAgent?.Height ?? 1000,
                UserInterfaceLink = rs.UserAgent != null ? rs.UserAgent.UserInterfaceLink ?? "" : String.Empty,
                UserInterfaceIsOpen = rs.UserAgent != null && rs.UserAgent.UserInterfaceIsOpen,
                UseScrollbars = rs.UserAgent != null && rs.UserAgent.UseScrollbars,
                Inputs = rs.UserAgent?.Inputs ?? 0,
                NrOfGpos = rs.UserAgent?.NrOfGpos ?? 0,
                InputGainStep = rs.UserAgent?.InputGainStep ?? 0,
                InputMaxDb = rs.UserAgent?.MaxInputDb ?? 0,
                InputMinDb = rs.UserAgent?.MinInputDb ?? 0,
                Lines = rs.UserAgent?.Lines ?? 0,
                
                CodecPresets = rs.UserAgent != null ? rs.UserAgent.CodecPresets.Select(MapToCodecPreset).ToList() :  new List<CodecPreset>(),

                LocationName = rs.Location != null ? rs.Location.Name : string.Empty,
                LocationComment = rs.Location != null ? rs.Location.Comment : string.Empty,
                CityName = rs.Location?.City != null ? rs.Location.City.Name : string.Empty,
                RegionName = rs.Location?.Region != null ? rs.Location.Region.Name : string.Empty,
            };

            return model;
        }

        private CodecPreset MapToCodecPreset(Entities.CodecPresetEntity codecPreset)
        {
            return new CodecPreset()
            {
                Id = codecPreset.Id,
                Name = codecPreset.Name
            };
        }

    }
}
