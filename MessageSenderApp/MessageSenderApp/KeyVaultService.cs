using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace MessageSenderApp;

public class KeyVaultService
{
    private readonly IConfiguration _configurationManager;
    private readonly SecretClient _secretClient;

    public KeyVaultService(IConfiguration configurationManager) 
    {
        _configurationManager = configurationManager;

        var keyVaultUrl = _configurationManager.GetValue<string>("KeyVaultUrl");
        
        _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

    }

    public string GetSecret(string secretName)
    {
        KeyVaultSecret secret = _secretClient.GetSecret(secretName);
        return secret.Value;
    }
}
