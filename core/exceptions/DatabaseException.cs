namespace core.exceptions;

public class DatabaseException : Exception
{
    public DatabaseException(string errorMessage) : base(errorMessage)
    {
    }
}
