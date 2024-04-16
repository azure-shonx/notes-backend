namespace net.shonx.privatenotes.backend.web;

using net.shonx.privatenotes.backend.handlers;

public class WebHandler
{
    private readonly WebApplicationBuilder builder;
    private readonly WebApplication app;

    private readonly RequestProcessor RequestProcessor;
    public WebHandler(string[] args)
    {
        builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();

        RequestProcessor = new RequestProcessor(new AzureKeyVaultHandler());

        BuildMappings();
    }

    public void Run()
    {
        app.Run();
    }

    private void BuildMappings()
    {
        app.MapGet("/", RequestProcessor.TestRoot).WithName("TestRoot");

        app.MapGet("/note/get/", RequestProcessor.GetNote).WithName("GetNote");

        app.MapGet("/note/getall/", RequestProcessor.GetAllNotes).WithName("GetAllNotes");

        app.MapPut("/note/create/", RequestProcessor.CreateNote).WithName("CreateNote");

        app.MapPut("/note/delete/", RequestProcessor.DeleteNote).WithName("DeleteNote");

        app.MapPost("/note/update/", RequestProcessor.UpdateNote).WithName("UpdateNote");
    }
}