namespace SampleCRM
{
    using Microsoft.Extensions.Configuration;

    public class AppSettings
    {
        public string StorageConnectionString { get; set; }

        public string TableName { get; set; }

        public string PartitionKey { get; set; }

        public string SigningSecurityKey { get; set; }

        public string AdminPassword { get; set; }

        public static AppSettings LoadAppSettings()
        {
            IConfigurationRoot configRoot = new ConfigurationBuilder()
                .AddJsonFile("Sensitive.json")
                .Build();
            AppSettings appSettings = configRoot.Get<AppSettings>();
            return appSettings;
        }
    }
}