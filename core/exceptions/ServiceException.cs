namespace core.exceptions;

public class ServiceException : Exception
{
    public ServiceException(string errorMessage) : base(errorMessage)
    {
    }
}
