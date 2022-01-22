namespace Wrapper.wrapper.Exceptions;

public class RequestFailedException: ImdbException
{
    public RequestFailedException(string? url)
        : base($"Request for url {url} failed")
    {
    }
}