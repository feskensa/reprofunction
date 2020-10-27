using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Repro.Functions.Loader
{
    public interface ILoader
    {
        Task<dynamic> LoadAsync(string dllBlobRef);
        void BindLogger(ILogger log);
    }
}
