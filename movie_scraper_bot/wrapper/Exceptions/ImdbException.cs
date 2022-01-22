namespace Wrapper.wrapper.Exceptions;

public abstract class ImdbException: Exception
{
    protected ImdbException(string? message = null)
        : base(message)
    {
    }
}