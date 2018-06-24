using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Exceptions
{
    public class CustomCodeException: ApplicationException
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
        = @"text/plain";

        public CustomCodeException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public CustomCodeException(int statusCode, string errorMessage) :
            base(errorMessage)
        {
            StatusCode = statusCode;
        }

        public CustomCodeException(int statusCode, Exception innerException)
            : this(statusCode, innerException.ToString())
        {
            StatusCode = statusCode;
        }

        public CustomCodeException(int statusCode, JObject errorObject)
            : this(statusCode, errorObject.ToString())
        {
            ContentType = @"application/json";
        }
    }
}
