using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public interface IStorageService
    {
        Task<bool> CreateContainerAsync(string containerName, bool isPublic);
        Task<bool> DeleteContainerAsync(string containerName);
        Task UploadBlobAsync(Stream fileStream, string blobNameToCreate, string containerName);
        IEnumerable<IListBlobItem> GetListBlobs(string containerName);
        Task<FileStream> DownloadBlobAsStreamAsync(FileStream fileStream, string containerName, string blobName);
        Task<string> DownloadBlobContentAsStringAsync(string containerName, string blobName);
        Task<byte[]> DownloadBlobContentAsByteArrayAsync(string containerName, string blobName);
        Task DeleteBlobAsync(string containerName, string blobName);

        Task CopyBlobAsync(string srcContainerName, string srcBlobName, string destContainerName, string destBlobName);
    }
}
