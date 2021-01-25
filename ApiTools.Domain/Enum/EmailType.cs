using System;

namespace ApiTools.Domain
{
    [Flags]
    public enum EmailType : byte
    {
        /// Banned address
        Banned = 0,
        /// Primary account address
        Primary = 1,
        /// Additional address
        Additional = 2,
        /// Recovery address
        Recovery = 4
    }
}
