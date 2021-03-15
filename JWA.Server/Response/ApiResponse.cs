using JWA.Core.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.Api.Response
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }
        public ApiResponse(T data, string message)
        {
            Data = data;
            Message = message;
        }
        public T Data { get; set; }
        public Metadata Meta { get; set; }
        public string Message { get; set; }
    }

    public class DeviceApiResponse<T>
    {
        public DeviceApiResponse(string message)
        {
            Message = message;
        }
        public DeviceApiResponse(T data, string message)
        {
            Data = data;
            Message = message;
        }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
