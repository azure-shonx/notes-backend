namespace net.shonx.privatenotes.backend;

using System.Text;
using Newtonsoft.Json;

public class TokenTester
{
    private readonly string Token;
    public Token.Header? Header { get; private set; }
    public Token.Payload? Payload { get; private set; }

    public TokenTester(string token) {
        this.Token = token;
    }

    public bool IsValidToken()
    {
        string[] parts = Token.Split(".");
        if (parts.Length != 3)
            return false;

        string? header = TryBase64Decode(parts[0]);
        string? payload = TryBase64Decode(parts[1]);
        if (string.IsNullOrEmpty(header) || string.IsNullOrEmpty(payload))
            return false;

        Header = JsonConvert.DeserializeObject<Token.Header>(header);
        Payload = JsonConvert.DeserializeObject<Token.Payload>(payload);
        if (Header is null || Payload is null)
            return false;

        return !Payload.isExpired();
    }

    /// <summary>
    /// Sometimes, Microsoft's tokens aren't padded properly. This method accounts for that.
    /// </summary>
    /// <param name="encoded">The string to try and decode.</param>
    /// <returns>The decoded value.</returns>
    private string? TryBase64Decode(string encoded)
    {
        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }
        catch (FormatException) { }
        try
        {
            encoded += "=";
            return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }
        catch (FormatException) { }
        return null;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}