namespace net.shonx.privatenotes.backend;

using System.Text.RegularExpressions;
using Newtonsoft.Json;

public partial class Note
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

    public bool IsValidName() {
        if(string.IsNullOrEmpty(Name))
            throw new NullReferenceException();
        bool Matches = MyRegex().IsMatch(Name);
        Console.WriteLine($"Is {Name} valid? {Matches}");
        return Matches;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    [GeneratedRegex(Program.REGEX)]
    private static partial Regex MyRegex();
}