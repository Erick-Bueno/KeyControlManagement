namespace keycontrol.Domain.Shared;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
     public bool IsFailure => !IsSuccess; 
    public string ErrorMessage { get; private set; }
    public T Value { get; private set; }

    private Result(bool isSuccess, T value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty); 
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(false, default, errorMessage); 
    }
}