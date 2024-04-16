namespace net.shonx.privatenotes.backend.requests;

using Newtonsoft.Json;

public abstract class Request
{
    public string SecretName { get; }

    public Request(string SecretName)
    {
        if (string.IsNullOrEmpty(SecretName))
            throw new NullReferenceException("SecretName");
        this.SecretName = SecretName;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}