namespace net.shonx.privatenotes.backend.requests;

public class GetNoteRequest : Request
{
    public GetNoteRequest(string SecretName) : base(SecretName) { }
}