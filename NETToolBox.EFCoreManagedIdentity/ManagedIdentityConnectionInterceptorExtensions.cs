using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using NETToolBox.EFCoreManagedIdentity;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore
{
    public static class ManagedIdentityConnectionInterceptorExtensions
    {
        /// <summary>
        /// UseManagedIdentity for this SqlConnection specifiying an optional tenantID taken from https://github.com/juunas11/Joonasw.ManagedIdentityDemos
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseManagedIdentity([NotNull] this DbContextOptionsBuilder optionsBuilder, string? tenantID = null)
        {
            var sqlServerOptionsExtension = optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>();
            if (sqlServerOptionsExtension == null) throw new NotImplementedException("UseManagedIdentity is only implemented for SQL Server");
            var connectionString = sqlServerOptionsExtension.ConnectionString;

            if (connectionString.Contains("user id=", StringComparison.OrdinalIgnoreCase) || connectionString.Contains("Integrated Security=", StringComparison.OrdinalIgnoreCase) || connectionString.Contains("user id =", StringComparison.OrdinalIgnoreCase) || connectionString.Contains("Integrated Security =", StringComparison.OrdinalIgnoreCase)) return optionsBuilder; //no-op if connectionString contains user id or integrated security

            optionsBuilder.AddInterceptors(new ManagedIdentityConnectionInterceptor(tenantID));
            return optionsBuilder;
        }
    }
}