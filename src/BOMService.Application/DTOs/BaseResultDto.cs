using System.Net;

namespace BOMService.Application.DTOs
{
    public class BaseResultDto<T>
    {
        public BaseResultDto(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public static BaseResultDto<T> Success(T data, string message = "")
        {
            return new BaseResultDto<T>(true, message, data);
        }

        public static BaseResultDto<T> Failure(string message, T? data = default)
        {
            return new BaseResultDto<T>(false, message, data);
        }
    }
}
