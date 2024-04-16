namespace net.shonx.privatenotes.backend.handlers;

using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

internal class InternalKeyVault
{
    private readonly SecretClient client = new(new Uri("https://private-notes.vault.azure.net"), new DefaultAzureCredential());
    internal InternalKeyVault() { }

    private async Task<KeyVaultSecret?> GetKVSecret(string secretName, bool checkId, string? id = null)
    {
        if (string.IsNullOrEmpty(secretName))
            return null;
        Azure.Response<KeyVaultSecret>? res = null;
        try
        {
            res = await client.GetSecretAsync(secretName);
        }
        catch (RequestFailedException e) when (e.Status == 404)
        {
            return null; // Why can't they just return a null response?
        }
        if (res == null)
            return null;
        KeyVaultSecret secret = res.Value;
        if (checkId)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException();
            string? SecretID;
            if (secret.Properties.Tags.TryGetValue("id", out SecretID))
            {
                if (id.Equals(SecretID))
                    return secret;
            }
            return null;
        }
        return secret;
    }

    internal Task<KeyVaultSecret?> GetKVSecret(string secretName, string id)
    {
        return GetKVSecret(secretName, true, id);
    }

    internal Task<KeyVaultSecret?> GetKVSecret(string secretName)
    {
        return GetKVSecret(secretName, false);
    }

    internal async Task<AKVHResponse> DeleteSecret(KeyVaultSecret secret)
    {
        if (secret is null)
            return AKVHResponse.NULL_REQUEST;
        DeleteSecretOperation DeleteSecretOperation = await client.StartDeleteSecretAsync(secret.Name);
        DeleteSecretOperation.WaitForCompletion();
        Response resP = await client.PurgeDeletedSecretAsync(secret.Name);
        if (resP.IsError)
            return AKVHResponse.NULL_RESPONSE;
        return AKVHResponse.OK;
    }

    internal async Task<AKVHResponse> SetSecret(KeyVaultSecret secret)
    {
        if (secret is null)
            return AKVHResponse.NULL_REQUEST;
        Response<KeyVaultSecret>? response = null;
        try
        {
            response = await client.SetSecretAsync(secret);
        }
        catch (RequestFailedException e) when (e.Status == 409)
        {
            return AKVHResponse.SECRET_NAME_TAKEN;
        }
        if (response is not null)
            return AKVHResponse.OK;
        return AKVHResponse.NULL_RESPONSE;
    }

    internal Azure.AsyncPageable<SecretProperties> GetPropertiesOfSecretsAsync()
    {
        return client.GetPropertiesOfSecretsAsync();
    }
}