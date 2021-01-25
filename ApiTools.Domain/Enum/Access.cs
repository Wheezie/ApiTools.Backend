using System;

namespace ApiTools.Domain.Enum
{
    [Flags]
    public enum Access : byte
    {
        None = 0,
        Read = 1,
        Write = 2,
        Moderate = 4,
        Delete = 8,

        All = Read | Write | Moderate | Delete
    }
}
