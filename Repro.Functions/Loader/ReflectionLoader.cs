using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Repro.Functions.Storage;

namespace Repro.Functions.Loader
{
    public class ReflectionLoader : ILoader
    {
        private readonly IStorage _blobStorage;
        private ILogger _logger;

        public ReflectionLoader(IStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public void BindLogger(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<dynamic> LoadAsync(string dllBlobRef)
        {
            TypeInfo modelTypeInfo = await ExtractTypeInfo(dllBlobRef);
            dynamic instance = Activator.CreateInstance(modelTypeInfo);

            return instance;
        }

        private async Task<TypeInfo> ExtractTypeInfo(string dllBlobRef)
        {
            string base64ModelBlobContent = await _blobStorage.DownloadTextAsync(dllBlobRef);
            byte[] assemblyByteArray = Convert.FromBase64String(base64ModelBlobContent);
            Assembly modelAssembly = Assembly.Load(assemblyByteArray);

            return modelAssembly.DefinedTypes.First();
        }
    }    
}
