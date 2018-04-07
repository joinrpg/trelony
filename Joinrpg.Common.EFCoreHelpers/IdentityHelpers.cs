using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Common.EFCoreHelpers
{
    public static class IdentityHelpers
    {
        public static Task EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
        public static Task DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);

        private static Task SetIdentityInsert<T>(DbContext context, bool enable)
        {
            var mapping = context.Model.FindEntityType(typeof(T)).Relational();
            var value = enable ? "ON" : "OFF";

            return context.Database.ExecuteSqlCommandAsync(
                $"SET IDENTITY_INSERT {mapping.Schema}.{mapping.TableName} {value}");
        }
    }
}
