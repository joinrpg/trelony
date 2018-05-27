namespace Joinrpg.Trelony.DataModel
{
    public class UserRole
    {
        public int UserRoleId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public UserRoleType RoleType { get; set; }

        public int? MacroRegionId { get; set; }
        public MacroRegion MacroRegion { get; set; }
    }
}
