using AutoMapper;
using CCM.Core.Entities;
using CCM.Data.Entities;
using CCM.Web.Models.Profile;
using CCM.Web.Models.Studio;
using CCM.Web.Models.UserAgents;
using System;
using System.Collections.Generic;
using System.Linq;
using CCM.Core.CodecControl.Entities;
using CCM.Web.Models.CodecControl;
using CCM.Web.Models.StudioMonitor;


namespace CCM.Web.Infrastructure
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<StudioEntity, Studio>();
                cfg.CreateMap<Studio, StudioViewModel>();
                cfg.CreateMap<StudioViewModel, Studio>()
                    .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                    .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedBy, opt => opt.Ignore());

                cfg.CreateMap<Studio, StudioMonitorViewModel>()
                .ForMember(x => x.StudioId, opt => opt.MapFrom(studio => studio.Id));

                cfg.CreateMap<ProfileGroupEntity, ProfileGroup>()
                    .ForMember(dest => dest.Profiles, opt => opt.MapFrom(scr => scr.OrderedProfiles.OrderBy(c => c.SortIndex).Select(c => c.Profile)));

                cfg.CreateMap<ProfileEntity, Core.Entities.Profile>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(scr => scr.Id))
                    .ForMember(dest => dest.Groups, opt => opt.MapFrom(scr => scr.ProfileGroups))
                    .ForMember(dest => dest.UserAgents, opt => opt.Ignore()); //fixa om den ska användas någon annanstans än vid profilegroup

                cfg.CreateMap<ProfileGroupProfileOrdersEntity, ProfileGroupInfo>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore());

                cfg.CreateMap<ProfileGroupInfo, ProfileGroupProfileOrdersEntity>()
                    .ForMember(x => x.ProfileGroupId, opt => opt.Ignore())
                    .ForMember(x => x.ProfileId, opt => opt.Ignore())
                    .ForMember(x => x.ProfileGroup, opt => opt.Ignore())
                    .ForMember(x => x.Profile, opt => opt.MapFrom(src => src))
                    .ForMember(x => x.SortIndex, opt => opt.Ignore());

                cfg.CreateMap<ProfileGroupInfo, ProfileEntity>()
                    .ForMember(dest => dest.Sdp, opt => opt.Ignore())
                    .ForMember(dest => dest.SortIndex, opt => opt.Ignore())
                    .ForMember(dest => dest.ProfileGroups, opt => opt.Ignore())
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.UserAgents, opt => opt.Ignore()) //fixa om den ska användas någon annanstans än vid profilegroup
                    .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                    .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedBy, opt => opt.Ignore());

                cfg.CreateMap<Core.Entities.Profile, ProfileEntity>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(scr => scr.Id))
                    .ForMember(dest => dest.ProfileGroups, opt => opt.MapFrom(scr => scr.Groups))
                    .ForMember(dest => dest.UserAgents, opt => opt.Ignore()); //fixa om den ska användas någon annanstans än vid profilegroup

                cfg.CreateMap<ProfileGroupEntity, ProfileGroupInfo>();
                cfg.CreateMap<ProfileGroupInfo, ProfileGroupEntity>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.OrderedProfiles, opt => opt.Ignore())
                    .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                    .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedBy, opt => opt.Ignore());

                cfg.CreateMap<ProfileGroup, ProfileGroupEntity>()
                    .ForMember(dest => dest.OrderedProfiles, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore());

                cfg.CreateMap<Core.Entities.Profile, ProfileGroupProfileOrdersEntity>()
                    .ForMember(x => x.ProfileGroupId, opt => opt.Ignore())
                    .ForMember(x => x.ProfileId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(x => x.ProfileGroup, opt => opt.Ignore())
                    .ForMember(x => x.Profile, opt => opt.MapFrom(src => src))
                    .ForSourceMember(x => x.CreatedOn, opt => opt.Ignore())
                    .ForSourceMember(x => x.CreatedBy, opt => opt.Ignore())
                    .ForSourceMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForSourceMember(x => x.UpdatedBy, opt => opt.Ignore());

                cfg.CreateMap<ProfileGroup, ProfileGroupViewModel>()
                    .ForMember(dest => dest.Profiles, opt => opt.MapFrom(scr => scr.Profiles));
                cfg.CreateMap<ProfileGroupViewModel, ProfileGroup>()
                    .ForMember(dest => dest.Profiles, opt => opt.MapFrom(scr => scr.Profiles.Where(p => p.Selected)))
                    .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                    .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedBy, opt => opt.Ignore());

                cfg.CreateMap<Core.Entities.Profile, ProfileListItemViewModel>()
                    .ForMember(dest => dest.Selected, opt => opt.MapFrom(scr => true));
                cfg.CreateMap<ProfileListItemViewModel, Core.Entities.Profile>()
                    .ForMember(x => x.Description, opt => opt.Ignore())
                    .ForMember(x => x.Sdp, opt => opt.Ignore())
                    .ForMember(x => x.Groups, opt => opt.Ignore())
                    .ForMember(x => x.UserAgents, opt => opt.Ignore())
                    .ForMember(x => x.CreatedOn, opt => opt.Ignore())
                    .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedBy, opt => opt.Ignore());

                cfg.CreateMap<AudioStatus, AudioStatusViewModel>()
                    .ForMember(x => x.Error, opt => opt.Ignore());
            });

            Mapper.Configuration.CompileMappings();
            Mapper.AssertConfigurationIsValid();

        }
    }
}