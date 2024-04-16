namespace net.shonx.privatenotes.backend;

public enum AKVHResponse
{
    OK,
    SECRET_NAME_TAKEN,
    NO_EMPTY_SECRET_NAME,
    NO_EMPTY_SECRET_VALUE,
    NULL_ID,
    NULL_REQUEST,
    NO_SUCH_SECRET,
    SECRET_MISMATCH,
    NULL_RESPONSE
}