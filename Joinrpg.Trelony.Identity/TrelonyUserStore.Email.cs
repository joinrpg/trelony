using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.Identity
{
    public partial class TrelonyUserStore
    {
        /// <inheritdoc />
        public async Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken)
        {
            var dbUser = await QueryUserById(user.UserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            dbUser.Email = email;
            await Context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        /// <inheritdoc />
        public async Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var dbUser = await QueryUserById(user.UserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return dbUser.EmailConfirmed;
        }

        /// <inheritdoc />
        public async Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
        {
            var dbUser = await QueryUserById(user.UserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            dbUser.EmailConfirmed = confirmed;
            await Context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return QueryUserByEmail(normalizedEmail).Select(GetIdentityUserSelector())
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return GetEmailAsync(user, cancellationToken);
        }

        /// <inheritdoc />
        public Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail,
            CancellationToken cancellationToken)
        {
            return SetEmailAsync(user, normalizedEmail, cancellationToken);
        }
    }
}