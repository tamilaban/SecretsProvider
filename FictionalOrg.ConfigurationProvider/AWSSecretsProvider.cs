using Amazon.SecretsManager.Model;
using FictionalOrg.ConfigurationProvider.Models;
using System;
using System.Threading.Tasks;

namespace FictionalOrg.ConfigurationProvider
{
    public class AWSSecretsProvider : IAWSSecretsProvider
    {
        private readonly IAWSClient _awsClient;
        private readonly ConfigProviderOptions _configProviderOptions;

        public AWSSecretsProvider(ConfigProviderOptions configProviderOptions, IAWSClient awsClient)
        {
            _configProviderOptions = configProviderOptions;
            _awsClient = awsClient;
        } 
        public Task<string> GetSecretAsync(string secretKey)
        {
            return _awsClient.GetAWSSecretsAsync(GetApplicationSecretId(secretKey));
        }

        public string GetSecret(string secretKey)
        {
            return _awsClient.GetAWSSecretsAsync(GetApplicationSecretId(secretKey)).Result;
        }

        private string GetApplicationSecretId(string configKey)
        {
            var formattedKey = string.Empty;
            var application = _configProviderOptions.ApplicationName;
            string environmentValue = string.Empty;
            switch (_configProviderOptions?.Environment?.ToLower())
            {
                //TODO : Update environment specific switch
                case "dev":
                    environmentValue = "Dev";
                    break;
            }
            if (!string.IsNullOrEmpty(application))
            {
                if (string.IsNullOrEmpty(environmentValue))
                {
                    configKey = $"{application}/{configKey}";
                }
                else
                {
                    configKey = $"{application}/{environmentValue}/{configKey}";
                }
            }
            return formattedKey;
        }


    }
}
