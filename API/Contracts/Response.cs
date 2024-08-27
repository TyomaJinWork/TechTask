using CSharpFunctionalExtensions;

namespace API.Contracts
{
    public class Response<T>
    {
        public T? Result { get; } = default;
        public string? Error { get; } = null;

        public Response(Result<T> resultObject)
        {
            Result = resultObject.IsSuccess ? resultObject.Value : default;
            Error = resultObject.IsFailure ? resultObject.Error : null;
        }

        public Response(T resultObject, string error)
        {
            Result = resultObject;
            Error = !string.IsNullOrEmpty(error) ? error : null;
        }
    }
}
