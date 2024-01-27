namespace core;

public class BaseApiResult
{
    public ErrorModel? Error { get; set; }
    public bool IsSuccess { get; set; } = true;
    public int Count { get; set; }
    public object? Data { get; set; }
}

public class ErrorModel
{
    public string? Message { get; set; }
    public string? ErrorCode { get; set; }
}
