using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace NETToolBox.EFCoreManagedIdentity
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
            optionsBuilder.AddInterceptors(new ManagedIdentityConnectionInterceptor(tenantID));
            return optionsBuilder;
        }
    }
}
