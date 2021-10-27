using Amazon.SecretsManager.Model;
using System.Threading.Tasks;

namespace FictionalOrg.ConfigurationProvider
{
    public interface IAWSClient
    {
        Task<string> GetAWSSecretsAsync(GetSecretValueRequest request);

        Task<string> GetAWSSecretsAsync(string configKey, string versionStage = "AWSCURRENT");
    }
}