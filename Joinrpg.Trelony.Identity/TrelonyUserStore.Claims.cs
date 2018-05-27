using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Joinrpg.Trelony.DataModel;
using Joinrpg.Trelony.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.Identity
{
    public partial class TrelonyUserStore
    {
        /// <inheritdoc />
        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var roles = await QueryUserById(user.UserId)
                .AsNoTracking()
                .SelectMany(x => x.Roles)
                .ToListAsync(cancellationToken);

            var macroRegionIds = await Context.MacroRegions.Select(mr => mr.MacroRegionId).ToListAsync(cancellationToken);

            return roles.SelectMany(ToClaim).ToList();

            IEnumerable<Claim> ToClaim(UserRole role)
            {
                switch (role.RoleType)
                {
                    case UserRoleType.Admin:
                        return new Claim[] {new AdminClaim()};
                    case UserRoleType.GlobalEditor:
                        return macroRegionIds.Select(macroRegionId => new EditorClaim(macroRegionId));
                    case UserRoleType.Editor when !(role.MacroRegionId is null):
                        return new Claim[] {new EditorClaim(role.MacroRegionId.Value),};
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        public Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim,
            CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}