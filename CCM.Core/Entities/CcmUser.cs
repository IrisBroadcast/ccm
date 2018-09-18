using CCM.Core.Entities.Base;
using CCM.Core.Interfaces;

namespace CCM.Core.Entities
{
    public class CcmUser : CoreEntityBase, ISipFilter
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"{UserName}\t{FirstName} {LastName}\t{Role}";
        }
    }
}