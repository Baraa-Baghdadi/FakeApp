using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.ApiResponse
{
    public interface IApiResponse
    {
        public Response<T> Success<T>(T data, Exception e);
        public Response<T> Success<T>(T data);
        public Response<T> Success<T>(T data, string msg);
        public Response<T> Unauthorized<T>(string msg);
        public Response<List<object>> Fail(string msg);
        public Response<T> Fail<T>(T data,string msg);
        public Response<T> Fail<T>(T message);
        public Response<T> Fail<T>(T data, Exception e);
        string GetExceptionMessage(Exception e);

    }
}
