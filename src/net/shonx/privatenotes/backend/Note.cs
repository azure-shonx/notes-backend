namespace net.shonx.privatenotes.backend;

using Newtonsoft.Json;

public class Note
{
    public string Name { get; }
    public string Value { get; }

    public Note(string name, string value)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            throw new NullReferenceException();
        this.Name = name;
        this.Value = value;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}