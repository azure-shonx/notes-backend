namespace net.shonx.privatenotes.backend.web;

using System.Net;
using net.shonx.privatenotes.backend.handlers;
using net.shonx.privatenotes.backend.requests;
using Newtonsoft.Json;

public class RequestProcessor
{
    private readonly AzureKeyVaultHandler keyVaultHandler;

    public RequestProcessor(AzureKeyVaultHandler keyVaultHandler)
    {
        this.keyVaultHandler = keyVaultHandler;
    }

    public async Task TestRoot(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        await WebUtil.WriteLines(context.Response, "{\"status\": \"OK\"}");
    }

    public async Task GetNote(HttpContext context)
    {
        GetNoteRequest? request = await WebUtil.GetRequest<GetNoteRequest>(context.Request);
        if (request is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        Identity? identity = WebUtil.ValidateToken(context.Request);
        if (identity is not null)
        {
            Note? secret = await keyVaultHandler.GetSecret(request.SecretName, identity.Payload.unique_name);
            if (secret is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                await WebUtil.WriteLines(context.Response, secret.ToString());
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }

    public async Task GetAllNotes(HttpContext context)
    {
        Identity? identity = WebUtil.ValidateToken(context.Request);
        if (identity is not null)
        {
            List<Note> secrets = await keyVaultHandler.GetSecrets(identity.Payload.unique_name);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await WebUtil.WriteLines(context.Response, JsonConvert.SerializeObject(secrets));
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }

    public async Task CreateNote(HttpContext context)
    {
        CreateNoteRequest? request = await WebUtil.GetRequest<CreateNoteRequest>(context.Request);
        if (request is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        Identity? identity = WebUtil.ValidateToken(context.Request);
        if (identity is not null)
        {
            AKVHResponse response = await keyVaultHandler.SetSecret(request.SecretName, request.SecretValue, identity.Payload.unique_name);
            if (response != AKVHResponse.OK)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await WebUtil.WriteLines(context.Response, response.ToString());
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
    public async Task DeleteNote(HttpContext context)
    {
        DeleteNoteRequest? request = await WebUtil.GetRequest<DeleteNoteRequest>(context.Request);
        if (request is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        Identity? identity = WebUtil.ValidateToken(context.Request);
        if (identity is not null)
        {
            AKVHResponse response = await keyVaultHandler.DeleteSecret(request.SecretName, identity.Payload.unique_name);
            if (response != AKVHResponse.OK)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await WebUtil.WriteLines(context.Response, response.ToString());
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
    public async Task UpdateNote(HttpContext context)
    {
        UpdateNoteRequest? request = await WebUtil.GetRequest<UpdateNoteRequest>(context.Request);
        if (request is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        Identity? identity = WebUtil.ValidateToken(context.Request);
        if (identity is not null)
        {
            AKVHResponse response = await keyVaultHandler.UpdateSecret(request, identity.Payload.unique_name);
            if (response != AKVHResponse.OK)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await WebUtil.WriteLines(context.Response, response.ToString());
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}