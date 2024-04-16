namespace net.shonx.privatenotes.backend.requests;

using Newtonsoft.Json;

public class UpdateNoteRequest : Request
{
    public string? NewName { get; }
    public string? NewValue { get; }

    public UpdateNoteRequest(string oldName, string? newName = null, string? newValue = null) : base(oldName)
    {
        this.NewName = newName;
        this.NewValue = newValue;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}