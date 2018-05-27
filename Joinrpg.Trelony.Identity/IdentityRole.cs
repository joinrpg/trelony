using Joinrpg.Trelony.DataModel;

namespace Joinrpg.Trelony.Identity
{
    class IdentityRole
    {
        public int IdentityRoleId { get; set; }

        public UserRoleType RoleType { get; set; }

        public int? MacroRegionId { get; set; }
    }
}
