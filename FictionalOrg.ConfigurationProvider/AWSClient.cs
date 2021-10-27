using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using FictionalOrg.ConfigurationProvider.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FictionalOrg.ConfigurationProvider
{
    public class AWSClient : IAWSClient
    {
        private readonly AmazonSecretsManagerClient _amazonSecretsManagerClient;

        public AWSClient(ConfigProviderOptions configProviderOptions)
        {
            _amazonSecretsManagerClient = GetAWSCredentialManager(configProviderOptions.AWSAccessKey, configProviderOptions.AWSSecretKey, configProviderOptions.Region);
        }

        private AmazonSecretsManagerClient GetAWSCredentialManager(string accessKey, string secretKey, string region)
        {
            AmazonSecretsManagerClient awsclient = null;
            var config = new AmazonSecretsManagerConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                awsclient = new AmazonSecretsManagerClient(config);
            }
            else
            {
                var cred = new BasicAWSCredentials(accessKey, secretKey);
                awsclient = new AmazonSecretsManagerClient(cred, config);
            }
            return awsclient;
        }

        private Task<GetSecretValueResponse> GetSecretsData(GetSecretValueRequest request)
        {
            return _amazonSecretsManagerClient.GetSecretValueAsync(request);
        }

        public async Task<string> GetAWSSecretsAsync(GetSecretValueRequest request)
        {
            var secretValue = string.Empty;
            try
            {
                var response = await GetSecretsData(request);
                if (response.SecretString != null)
                {
                    secretValue = response.SecretString;
                }
                else
                {
                    using (var reader = new StreamReader(response.SecretBinary))
                    {
                        secretValue = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                    }

                    var keyValuePair = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretValue).FirstOrDefault();
                    return keyValuePair.Value;
                }
            }
            catch (Exception ex)
            {
                ex = null;
                secretValue = string.Empty;
            }
            return secretValue;
        }

        public Task<string> GetAWSSecretsAsync(string secretId, string versionStage = "AWSCURRENT")
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretId,
                VersionStage = versionStage
            };

            return GetAWSSecretsAsync(request);
        }
    }
}
