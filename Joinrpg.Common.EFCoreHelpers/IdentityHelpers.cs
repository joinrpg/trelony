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
            var schema = mapping.Schema == null ? "" : (mapping.Schema + ".");
            var value = enable ? "ON" : "OFF";

            var rawSqlString = $"SET IDENTITY_INSERT {schema}{mapping.TableName} {value}";

            return context.Database.ExecuteSqlCommandAsync(rawSqlString);
        }
    }
}
