namespace net.shonx.privatenotes.backend;

using Newtonsoft.Json;

public class Identity
{
    public Token.Header Header { get; }
    public Token.Payload Payload { get; }

    public Identity(TokenTester request)
    {
        this.Header = request.Header ?? throw new NullReferenceException();
        this.Payload = request.Payload ?? throw new NullReferenceException();
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
