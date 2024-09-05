using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.ApiResponse
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public ResponseObject<T>? Data { get; set; }

    }
    public class ResponseObject<T>
    {
        public T Result { get; set; }
    }
}
