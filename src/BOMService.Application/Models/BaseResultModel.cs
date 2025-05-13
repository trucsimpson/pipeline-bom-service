using System.Net;

namespace BOMService.Application.Models
{
    public class BaseResultModel<T>
    {
        public BaseResultModel(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public static BaseResultModel<T> Success(T data, string message = "")
        {
            return new BaseResultModel<T>(true, message, data);
        }

        public static BaseResultModel<T> Failure(string message, T? data = default)
        {
            return new BaseResultModel<T>(false, message, data);
        }
    }
}
