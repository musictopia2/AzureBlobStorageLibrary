namespace AzureBlobStorageLibrary;
public class AzureBlobStorageService(BlobContainerClient containerClient) : IBlobStorageService
{
    Task IBlobStorageService.DeleteAsync(string blobName)
    {
        throw new NotImplementedException();
    }

    Task<Stream> IBlobStorageService.DownloadAsync(string blobName)
    {
        throw new NotImplementedException();
    }

    Task<bool> IBlobStorageService.ExistsAsync(string blobName)
    {
        throw new NotImplementedException();
    }

    Uri IBlobStorageService.GetUri(string blobName)
    {
        throw new NotImplementedException();
    }

    Task IBlobStorageService.UploadAsync(string blobName, Stream data)
    {
        throw new NotImplementedException();
    }
}