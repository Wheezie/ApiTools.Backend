#if DEBUG
using System;
using System.Reflection;

namespace ApiTools.Domain
{
    public static class XUnitRunner
    {
        public static bool IsTesting()
        {
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assem.FullName.ToLowerInvariant().Contains("xunit"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif