using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Repro.Functions;
using Repro.Functions.Loader;
using Repro.Functions.Storage;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Repro.Functions
{
    [ExcludeFromCodeCoverage]

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString =
                "DefaultEndpointsProtocol=https;AccountName=functionreprostorage;AccountKey=epa4m3SsIM1ql46LU6TVIcaWO2Nj9dnZJhAQMLLmo9G03WymgEzJqz4rsWFMhxtxWET6wbIJigaqAO2XWwzHbQ==;EndpointSuffix=core.windows.net";

            builder.Services.AddLogging();
            builder.Services.AddSingleton<ILoader>(model => {
                var blobStorage = new BlobStorage(CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient());

                return new ReflectionLoader(blobStorage);
            });
        }
    }
}
