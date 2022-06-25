// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

// ReSharper disable once UnusedType.Global
public static class DotNetUtility
{
    public static bool IsDebugMode()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
}