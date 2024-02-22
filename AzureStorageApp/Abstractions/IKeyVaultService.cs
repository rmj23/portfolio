/// <summary>
/// Interface for KeyVaultService
/// </summary>
public interface IKeyVaultService
{
    /// <summary>
    /// Get secret from KeyVault
    /// </summary>
    /// <param name="secretName">The Secret's name</param>
    /// <returns>The Secret value. Empty string if nothing found.</returns>
    string GetSecret(string secretName);
}