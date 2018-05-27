using System;
using System.Security.Claims;

namespace Joinrpg.Trelony.Identity.Models
{
    public class AdminClaim : Claim
    {
        public AdminClaim() : base("AdminRole", value: null)
        {
            
        }
    }
}
