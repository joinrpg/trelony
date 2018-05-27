using System.Security.Claims;

namespace Joinrpg.Trelony.Identity.Models
{
    public class EditorClaim : Claim
    {
        public EditorClaim(int macroRegionId) : base("EditorRole", macroRegionId.ToString())
        {

        }
    }
}