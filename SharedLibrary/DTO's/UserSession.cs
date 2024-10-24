using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTO_s
{
    public record UserSession(string? Id, string? Name, string? Email, string? Role);
    
}
