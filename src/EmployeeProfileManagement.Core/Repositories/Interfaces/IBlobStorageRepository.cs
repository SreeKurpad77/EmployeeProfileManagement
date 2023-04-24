using EmployeeProfileManagement.Core.Model;
using Microsoft.AspNetCore.Http;

namespace EmployeeProfileManagement.Core.Repositories.Interfaces
{
    public interface IBlobStorageRepository
    {
        /// <summary>
        /// This method uploads a file submitted with the request
        /// </summary>
        /// <param name="file">File for upload</param>
        /// <returns>Blob with status</returns>
        Task<BlobStorageResult> UploadAsync(IFormFile file);

        /// <summary>
        /// This method downloads a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob</returns>
        Task<BlobStorageRequest> DownloadAsync(string blobFilename);

        /// <summary>
        /// This method deleted a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob with status</returns>
        Task<BlobStorageResult> DeleteAsync(string blobFilename);

        /// <summary>
        /// This method returns a list of all files located in the container
        /// </summary>
        /// <returns>Blobs in a list</returns>
        Task<List<BlobStorageRequest>> ListAsync();
    }
}
