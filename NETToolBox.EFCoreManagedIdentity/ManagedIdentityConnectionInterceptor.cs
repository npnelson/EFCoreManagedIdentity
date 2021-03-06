﻿using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NETToolBox.EFCoreManagedIdentity
{
    internal class ManagedIdentityConnectionInterceptor : DbConnectionInterceptor
    {
        private readonly string? _tenantId;
        private readonly AzureServiceTokenProvider _tokenProvider;

        public ManagedIdentityConnectionInterceptor(string? tenantID)
        {
            if (string.IsNullOrEmpty(tenantID))
            {
                _tenantId = null;
            }
            else
            {
                _tenantId = tenantID;
            }

            _tokenProvider = new AzureServiceTokenProvider();
        }

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            return ConnectionOpeningAsync(connection, eventData, result).GetAwaiter().GetResult();
        }
        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
            DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            // Have to cast DbConnection to SqlConnection
            // AccessToken property does not exist on the base class
            var sqlConnection = (SqlConnection)connection;
            string accessToken = await GetAccessTokenAsync().ConfigureAwait(false);
            sqlConnection.AccessToken = accessToken;
            return result;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            string resource = "https://database.windows.net/";
            return await _tokenProvider.GetAccessTokenAsync(resource, _tenantId).ConfigureAwait(false);
        }
    }
}
