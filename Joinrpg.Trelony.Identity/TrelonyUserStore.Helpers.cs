using System;
using System.Linq;
using System.Linq.Expressions;
using Joinrpg.Trelony.DataModel;

namespace Joinrpg.Trelony.Identity
{
    public partial class TrelonyUserStore
    {
        private static Expression<Func<User, IdentityUser>> GetIdentityUserSelector() =>
            user => new IdentityUser()
            {
                UserId = user.UserId,
                UserName = user.UserName
            };

        private IQueryable<User> QueryUserById(int userIdInt) => Context.Users.Where(user => user.UserId == userIdInt);

        private IQueryable<User> QueryUserByEmail(string email) => Context.Users.Where(user => user.Email == email);

    }
}