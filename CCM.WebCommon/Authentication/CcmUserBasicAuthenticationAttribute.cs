using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using CCM.Core.Entities;
using CCM.Core.Interfaces.Repositories;
using CCM.Data.Repositories;
using LazyCache;

namespace CCM.WebCommon.Authentication
{
    public class CcmUserBasicAuthenticationAttribute : BasicAuthenticationAttributeBase
    {
        // Används i api-metoder för användarhantering/call/hangup. Authenticerar ett CCM-konto

        public ICcmUserRepository UserRepository => new CcmUserRepository(new CachingService());
        //[Inject]
        //public ICcmUserRepository UserRepository { get; set; }

        //protected CcmUser GetUser(string userName)
        //{
        //    return UserRepository.GetByUserName(userName);
        //}

        //protected CcmRole GetUserRoles(CcmUser user)
        //{
        //    return UserRepository.GetUserRole(user);
        //}

        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var userRepository = UserRepository;
            
            if (!await userRepository.AuthenticateAsync(userName, password))
            {
                return null;
            }

            var user = userRepository.GetByUserName(userName);

            if (user == null)
            {
                return null;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationTypes.Basic));
            return claimsPrincipal;
        }
    }
}