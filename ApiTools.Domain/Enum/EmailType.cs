using System;

namespace ApiTools.Domain
{
    [Flags]
    public enum EmailType : byte
    {
        /// Banned address
        Banned = 1,
        /// Primary account address
        Primary = 2,
        /// Additional address
        Additional = 4,
        /// Recovery address
        Recovery = 8
    }
}
