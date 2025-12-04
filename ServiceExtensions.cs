namespace AzureBlobStorageLibrary;
public static class ServiceExtensions
{
    extension (IServiceCollection services)
    {
        /// <summary>
        /// This method demonstrates how to register Azure Blob Storage using hardcoded (fake/mock) configuration values.
        /// It is intended for educational purposes only — to show what keys and setup are required
        /// (e.g., ConnectionString and ContainerName).
        ///
        /// ❗ Do not use this method in production environments. 
        /// If used with the fake credentials provided, any Azure Blob operations will fail at runtime.
        ///
        /// Instead, in a real app, supply valid values from secure configuration sources such as:
        /// - appsettings.json
        /// - environment variables
        /// - Azure Key Vault
        /// - Custom File Access (including tab delimited files)
        /// /// This method is especially useful for open-source projects where contributors might wire in their own config
        /// files (e.g., via a custom tab-delimited provider), while keeping the main source tree generic and credential-free.
        /// </summary>
        public IServiceCollection RegisterAzureBlobStorageWithFakeConfig()
        {
            // Manually create configuration values for AzureStorage (instead of using JSON files)
            var fakeConfigValues = new Dictionary<string, string>
        {
            { "AzureStorage:ConnectionString", "DefaultEndpointsProtocol=https;AccountName=youraccount;AccountKey=yourkey;EndpointSuffix=core.windows.net" },
            { "AzureStorage:ContainerName", "your-container-name" }
        };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(fakeConfigValues!)
                .Build();

            // Set globally for code that depends on bb1.Configuration
            bb1.Configuration = configuration;

            // Bind strongly typed config object
            var azureStorageConfig = configuration.GetSection("AzureStorage").Get<AzureStorageConfig>()
                ?? throw new CustomBasicException("AzureStorage configuration section is missing or invalid.");

            // Set up and prepare the blob container
            var blobServiceClient = new BlobServiceClient(azureStorageConfig.ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureStorageConfig.ContainerName);
            blobContainerClient.CreateIfNotExists();
            blobContainerClient.SetAccessPolicy(PublicAccessType.None);

            // Register dependencies
            services.AddSingleton(blobContainerClient);
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();

            return services;
        }
        public IServiceCollection RegisterBlobStorageClient()
        {
            var configuration = bb1.Configuration ?? throw new CustomBasicException("Needs IConfiguration Registered");
            var azureStorageConfig = configuration.GetSection("AzureStorage").Get<AzureStorageConfig>()
                ?? throw new CustomBasicException("AzureStorage configuration section is missing or invalid.");
            var blobServiceClient = new BlobServiceClient(azureStorageConfig.ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureStorageConfig.ContainerName);
            // Ensure container exists and configure access
            blobContainerClient.CreateIfNotExists();
            blobContainerClient.SetAccessPolicy(PublicAccessType.None);
            // Register the BlobContainerClient and AzureBlobStorageService
            services.AddSingleton(blobContainerClient);
            services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
            return services;
        }
    }
}