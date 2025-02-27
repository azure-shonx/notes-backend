namespace net.shonx.privatenotes.backend.web;

using System.Text;
using Newtonsoft.Json;
using net.shonx.privatenotes.backend.requests;

internal static class WebUtil
{
    internal static async Task<T?> GetRequest<T>(HttpRequest? request) where T : Request
    {
        if (request is null || request.ContentType == null)
        {
            Console.WriteLine("The request or request ContentType is null");
            return default(T);
        }
        if (!request.ContentType.Contains("application/json"))
        {
            Console.WriteLine($"Wrong ContentType. Content Type is {request.ContentType}");
            return default(T);
        }
        string json = await ReadLines(request.Body);
        try
        {
            T? obj = JsonConvert.DeserializeObject<T>(json);
            if (obj is null)
            {
                Console.WriteLine("JSON decoded object returned null.");
                return default(T);
            }
            return obj;
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception decoding JSON string.");
            Console.WriteLine(e.ToString());
            return default(T);
        }
    }

    internal static Identity? ValidateToken(HttpRequest request)
    {
        try
        {
            // Assuming you have an HttpContext available
            string authorizationHeader = request.Headers["Authorization"].ToString();
            if (authorizationHeader.StartsWith("Bearer"))
            {
                string accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();
                // Now you can use the accessToken for authentication
                TokenTester authRequest = new(accessToken);
                if (authRequest.IsValidToken())
                {
                    return new Identity(authRequest);
                }
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }

    }

    internal static async Task WriteLines(HttpResponse Response, string JSON)
    {
        Response.ContentType = "application/json; charset=utf-8";
        await using var writer = new StreamWriter(Response.Body, Encoding.UTF8);
        await writer.WriteAsync(JSON);
    }

    internal static async Task<string> ReadLines(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}