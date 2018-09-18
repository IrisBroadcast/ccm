using System.Collections.Generic;
using CCM.Core.Entities;

namespace CCM.Core.Interfaces.Repositories
{
    public interface IRadiusUserRepository
    {
        List<RadiusUser> GetUsers();
        List<UserInfo> GetSipAccountPasswords();
        List<UserInfo> GetUserPasswords();
        List<Dictionary<string, object>> GetUsersInfo();
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}