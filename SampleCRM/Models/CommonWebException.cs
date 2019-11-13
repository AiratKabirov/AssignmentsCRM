using System;
using System.Net;

namespace SampleCRM.Models
{
    public class CommonWebException : Exception
    {
        public readonly HttpStatusCode StatusCode;

        public CommonWebException()
        {
        }

        public CommonWebException(string message)
            : base(message)
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
        }

        public CommonWebException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        public CommonWebException(string message, Exception inner)
            : base(message, inner)
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
