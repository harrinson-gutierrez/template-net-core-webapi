using Infrastructure.Adapter.Storage.Models;
using System.Threading.Tasks;

namespace Infrastructure.Adapter.Storage.Interfaces
{
    public interface IStorageService
    {
        Task<StorageResponse> UploadObject(StorageRequest storageRequest);
        Task<StorageResponse> RemoveObject(StorageDeleteRequest storageDeleteRequest);
    }
}
