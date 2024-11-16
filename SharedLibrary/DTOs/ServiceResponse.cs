using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class ServiceResponse
    {
        public record GeneralResponse(bool Flag, string Message);
        public record LoginResponse(bool Flag, string Token, string Message);

    }
}
