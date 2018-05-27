using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.Identity
{
    public partial class TrelonyUserStore
    {
        public async Task SetPasswordHashAsync(IdentityUser user, string passwordHash,
            CancellationToken cancellationToken)
        {
            var dbUser = await QueryUserById(user.UserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            dbUser.PasswordHash = passwordHash;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return QueryUserById(user.UserId).Select(u => u.PasswordHash)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return QueryUserById(user.UserId).Select(u => u.PasswordHash != null)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}