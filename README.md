# EFCoreManagedIdentity
ManagedIdentityInterceptor based on https://github.com/juunas11/Joonasw.ManagedIdentityDemos
Use this until EFCore supports ManagedIdentity natively https://github.com/dotnet/efcore/issues/13261

```
 services.AddDbContext<YourDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("YourConnectionString")).UseManagedIdentity(); 
            });
```

ConnectionString should look something like this: ```Server=tcp:yourAzureServer.database.windows.net,1433;Initial Catalog=YourDatabase```

This link is also helpful https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-connect-msi (although has a bunch of extra information and defintely don't grab the token the way they do, that's what .UseManagedIdentity() is for)


