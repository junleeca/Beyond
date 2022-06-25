// ReSharper disable once CheckNamespace

namespace Beyond.Extensions.ByteExtended;

public static class ByteExtensions
{
    public static string ToHexString(this byte @byte)
    {
        return @byte.ToString("x2");
    }
}