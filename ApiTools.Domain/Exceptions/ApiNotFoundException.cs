using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Exceptions
{
    public class ApiNotFoundException : ApiToolsException
    {
        public ApiNotFoundException(string message, bool logged = false) : base(message, logged)
        {
        
        }
    }
}
