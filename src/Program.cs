using net.shonx.privatenotes.backend.web;

public class Program
{
  public static void Main(string[] args)
  {
   // Test.Run();
    WebHandler WebHandler = new(args);
    WebHandler.Run();
  }
}

