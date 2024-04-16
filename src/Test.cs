using System.Text.Json;

using net.shonx.privatenotes.backend.requests;

internal static class Test
{
    private static readonly CreateNoteRequest CreateNoteRequest = new("myNewSecret", "Pepsi");
    private static readonly GetNoteRequest GetNoteRequest = new("myNewSecret");
    private static readonly UpdateNoteRequest UpdateNameRequest = new("myNewSecret", "mySuperSecret", null);
    private static readonly GetNoteRequest GetNoteNewNameRequest = new("mySuperSecret");
    private static readonly UpdateNoteRequest UpdateValueRequest = new("mySuperSecret", null, "Adidas");
    private static readonly GetNoteRequest GetNoteNewValueRequest = new("mySuperSecret");
    private static readonly DeleteNoteRequest DeleteNoteRequest = new("mySuperSecret");

    internal static void Run()
    {
        Directory.Delete("test", true);
        Directory.CreateDirectory("test");
        JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };
        File.WriteAllText("test/1_CreateNoteRequest.json", JsonSerializer.Serialize(CreateNoteRequest, JsonSerializerOptions));
        File.WriteAllText("test/2_GetNoteRequest.json", JsonSerializer.Serialize(GetNoteRequest, JsonSerializerOptions));
        File.WriteAllText("test/3_UpdateNameRequest.json", JsonSerializer.Serialize(UpdateNameRequest, JsonSerializerOptions));
        File.WriteAllText("test/4_GetNoteNewNameRequest.json", JsonSerializer.Serialize(GetNoteNewNameRequest, JsonSerializerOptions));
        File.WriteAllText("test/5_UpdateValueRequest.json", JsonSerializer.Serialize(UpdateValueRequest, JsonSerializerOptions));
        File.WriteAllText("test/6_GetNoteNewValueRequest.json", JsonSerializer.Serialize(GetNoteNewValueRequest, JsonSerializerOptions));
        File.WriteAllText("test/7_DeleteNoteRequest.json", JsonSerializer.Serialize(DeleteNoteRequest, JsonSerializerOptions));

    }
}