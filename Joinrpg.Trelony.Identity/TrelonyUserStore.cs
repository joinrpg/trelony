using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Joinrpg.Trelony.DataAccess;
using Joinrpg.Trelony.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.Identity
{
    [UsedImplicitly]
    public partial class TrelonyUserStore : IUserPasswordStore<IdentityUser>, 
        IUserEmailStore<IdentityUser>,
        IUserClaimStore<IdentityUser>
    {
        public TrelonyUserStore(TrelonyContext context)
        {
            Context = context;
        }

        private TrelonyContext Context { get; }

        public void Dispose()
        {
            //It's job for DI to dispose context
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotSupportedException(); // Not sure that's it required
        }

        public Task<string> GetNormalizedUserNameAsync([NotNull] IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException(); // Not sure that's it required
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var dbUser = new User()
            {
                Email = user.Email,
                EmailConfirmed = false,
                RegistrationDate = DateTimeOffset.UtcNow,
                UserName = user.UserName,
            };

            await Context.Users.AddAsync(dbUser, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
            //Do we really need it?
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (!int.TryParse(userId, out var userIdInt))
            {
                return Task.FromResult<IdentityUser>(result: null);
            }

            return QueryUserById(userIdInt)
                .Select(GetIdentityUserSelector())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Context.Users
                .Where(user => user.Email == normalizedUserName)
                .Select(GetIdentityUserSelector())
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}