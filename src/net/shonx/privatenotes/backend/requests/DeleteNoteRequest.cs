namespace net.shonx.privatenotes.backend.requests;

public class DeleteNoteRequest : Request
{
    public DeleteNoteRequest(string SecretName) : base(SecretName) { }
}