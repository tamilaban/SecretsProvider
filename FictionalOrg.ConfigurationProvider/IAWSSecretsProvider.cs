using System.Threading.Tasks;

namespace FictionalOrg.ConfigurationProvider
{
    public interface IAWSSecretsProvider
    {
        Task<string> GetSecretAsync(string secretKey);
        string GetSecret(string secretKey);
    }
}