namespace net.shonx.privatenotes.backend.handlers;

using Azure.Security.KeyVault.Secrets;
using net.shonx.privatenotes.backend.requests;

public class AzureKeyVaultHandler
{
    private readonly InternalKeyVault client = new();
    public AzureKeyVaultHandler() { }

    public async Task<Note?> GetSecret(string secretName, string id)
    {
        if (string.IsNullOrEmpty(secretName))
            throw new NullReferenceException();
        if (string.IsNullOrEmpty(id))
            throw new NullReferenceException();
        KeyVaultSecret? secret = await client.GetKVSecret(secretName, id);
        if (secret is null)
            return null;
        return new Note(secretName, secret.Value);
    }

    public async Task<List<Note>> GetSecrets(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new NullReferenceException();
        List<Note> notes = new(0);
        await foreach (SecretProperties item in client.GetPropertiesOfSecretsAsync())
        {
            KeyVaultSecret? secret = await client.GetKVSecret(item.Name, id);
            if (secret is not null)
                notes.Add(new Note(secret.Name, secret.Value));
        }
        return notes;
    }

    private async Task<bool> SecretExists(string secretName)
    {
        if (string.IsNullOrEmpty(secretName))
            throw new NullReferenceException();
        KeyVaultSecret? secret = await client.GetKVSecret(secretName);
        return secret is not null;
    }


    public async Task<AKVHResponse> SetSecret(string secretName, string secretValue, string id)
    {
        if (string.IsNullOrEmpty(secretName))
            return AKVHResponse.NO_EMPTY_SECRET_NAME;
        if (string.IsNullOrEmpty(secretValue))
            return AKVHResponse.NO_EMPTY_SECRET_VALUE;
        if (string.IsNullOrEmpty(id))
            return AKVHResponse.NULL_ID;
        if (await SecretExists(secretName))
            return AKVHResponse.SECRET_NAME_TAKEN;

        KeyVaultSecret secret = new(secretName, secretValue);
        secret.Properties.Tags["id"] = id;
        return await client.SetSecret(secret);
    }

    public async Task<AKVHResponse> UpdateSecret(UpdateNoteRequest request, string id)
    {
        if (request is null)
            return AKVHResponse.NULL_REQUEST;
        if (string.IsNullOrEmpty(id))
            return AKVHResponse.NULL_ID;
        KeyVaultSecret? secret = await client.GetKVSecret(request.SecretName, id);
        if (secret is null)
            return AKVHResponse.NO_SUCH_SECRET;

        string Name = request.NewName ?? secret.Name;
        string Value = request.NewValue ?? secret.Value;

        // Only delete on new name.
        if (!Name.Equals(secret.Name))
        {
            AKVHResponse res = await client.DeleteSecret(secret);
            if (res != AKVHResponse.OK)
                throw new InvalidOperationException("Delete unsuccessful.");
        }

        KeyVaultSecret newSecret = new(Name, Value);
        newSecret.Properties.Tags["id"] = id;
        return await client.SetSecret(newSecret);

    }

    public async Task<AKVHResponse> DeleteSecret(string secretName, string id)
    {
        if (string.IsNullOrEmpty(secretName))
            return AKVHResponse.NO_EMPTY_SECRET_NAME;
        if (string.IsNullOrEmpty(id))
            return AKVHResponse.NULL_ID;
        KeyVaultSecret? secret = await client.GetKVSecret(secretName, id);
        if (secret is null)
            return AKVHResponse.NO_SUCH_SECRET;
        return await client.DeleteSecret(secret);
    }


}