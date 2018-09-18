using CCM.Core.Entities.Specific;
using CCM.Core.Helpers;
using CCM.Web.Models.Home;

namespace CCM.Web.Mappers
{
    public static class RegisteredSipOverviewDtoMapper
    {
        public static RegisteredSipOverviewDto MapToDto(RegisteredSipDto regSip, string sipDomain)
        {
            return new RegisteredSipOverviewDto
            {
                InCall = regSip.InCall,
                Sip = regSip.Sip,
                Id = regSip.Id,
                //DisplayName = regSip.DisplayName,


                DisplayName = DisplayNameHelper.GetDisplayName(regSip.DisplayName, regSip.UserDisplayName, 
                string.Empty, regSip.UserName, regSip.Sip, "", sipDomain),


                Location = regSip.LocationName,
                LocationShortName = regSip.LocationShortName,
                Comment = regSip.Comment,
                Image = regSip.Image,
                CodecTypeName = regSip.CodecTypeName,
                CodecTypeColor = regSip.CodecTypeColor,
                UserName = regSip.UserName,
                UserDisplayName = regSip.UserDisplayName,
                UserComment = regSip.Comment,
                InCallWithId = regSip.InCallWithId,
                InCallWithSip = DisplayNameHelper.AnonymizePhonenumber(regSip.InCallWithSip),
                InCallWithName = DisplayNameHelper.AnonymizeDisplayName(regSip.InCallWithName),
                RegionName = regSip.RegionName,
                Updated = regSip.Updated
            };
        }

    }
}