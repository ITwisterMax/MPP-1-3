using System;

namespace AssemblyBrowserLibrary.Helper
{
    /// <summary>
    ///     Permissions list
    /// </summary>
    [Flags]
    public enum Permissions
    {
        Abstract = 1,
        Virtual = 2,
        Static = 4,
        Sealed = 8,
        Readonly = 16
    }
}
