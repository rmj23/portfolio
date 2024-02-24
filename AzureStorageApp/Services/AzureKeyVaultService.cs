using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Ardalis.GuardClauses;

/// <summary>
/// Implementation of the KeyVaultService
/// </summary>
public class AzureKeyVaultService : IKeyVaultService
{
    /// <summary>
    /// The secret client
    /// </summary>
    private readonly SecretClient _client;

    /// <summary>
    /// Initializes a new instance of the KeyVaultService class. Client gets created once and reused.
    /// </summary>
    /// <param name="config">Configuration</param>
    public AzureKeyVaultService(IConfiguration config)
    {
        string? keyVaultUrl = config.GetConnectionString("KeyVault");
        string? managedIdentityClientId = config.GetValue<string>("ManagedIdentityClientId");

        Guard.Against.NullOrEmpty(keyVaultUrl, nameof(keyVaultUrl));
        Guard.Against.NullOrEmpty(managedIdentityClientId, nameof(managedIdentityClientId));

        var creds = new DefaultAzureCredential(new DefaultAzureCredentialOptions 
        {
            ManagedIdentityClientId = managedIdentityClientId
        });
        
        _client = new SecretClient(new Uri(keyVaultUrl), creds);
    }

    /// <summary>
    /// Get the secret from the key vault
    /// </summary>
    /// <param name="secretName">The secret name</param>
    /// <returns>The secret's value. Empty string if noting found</returns>
    public async Task<string> GetSecretAsync(string secretName)
    {
        try
        {
            var result = await _client.GetSecretAsync(secretName);
            return result.Value.Value ?? string.Empty;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred while getting the secret: {ex.Message}");
            return string.Empty;
        }
    }
}