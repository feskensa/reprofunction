using System.Threading.Tasks;

namespace Repro.Functions.Storage
{
    public interface IStorage
    {
        Task<string> DownloadTextAsync(string storageRef);
    }
}
