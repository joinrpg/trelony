using System;
using System.Collections.Generic;

namespace Joinrpg.Trelony.DataModel
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public DateTimeOffset RegistrationDate { get; set; }
        public bool EmailConfirmed { get; set; }

        public ISet<UserRole> Roles { get; set; } = new HashSet<UserRole>();
    }
}
