using FluentValidation.Results;

namespace core.exceptions;

public class ApiValidationException : Exception
{
    public List<string> Failures { get; }

    public ApiValidationException() : base("One or more validation failures have occurred!")
    {
        Failures = new List<string>();
    }

    public ApiValidationException(string message) : this() => Failures.Add(message);

    public ApiValidationException(List<ValidationFailure> failures) : this()
    {
        var propertyNames = failures.Select(e => e.PropertyName).Distinct();

        foreach (var propertyName in propertyNames)
        {
            var propertyFailures = failures
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            Failures.Add($"{string.Join(", ", propertyFailures)}");
        }
    }
}
