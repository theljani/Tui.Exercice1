using System;
using System.Collections.Generic;
using System.Text;

namespace TUI.Flights.Common.Enums
{
    public enum StatusCodes
    {
        NotFound = 404,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        InternalServerError = 500
    }
}
