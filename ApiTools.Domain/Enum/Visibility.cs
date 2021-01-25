using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools.Domain.Enum
{
    [Flags]
    public enum Visibility : byte
    {
        Private = 0,
        Public = 1,
        Friends = 2,
        Members = 4,
        Deleted = 8
    }
}
