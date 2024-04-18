using net.shonx.privatenotes.backend.web;

public class Program
{
  public const string REGEX = @"^[A-Za-z0-9-]+$";
  public static void Main(string[] args)
  {
    WebHandler WebHandler = new(args);
    WebHandler.Run();
  }
}

