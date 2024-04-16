namespace net.shonx.privatenotes.backend.requests;

public class CreateNoteRequest : Request
{
    public string SecretValue { get; }

    public CreateNoteRequest(string SecretName, string secretValue) : base(SecretName)
    {
        if (string.IsNullOrEmpty(secretValue))
            throw new NullReferenceException();
        this.SecretValue = secretValue;
    }
}