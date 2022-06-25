// ReSharper disable once CheckNamespace

// ReSharper disable UnusedMember.Global
namespace Beyond.Extensions.FileSignature;

// Site: https://www.filesignatures.net/
// ReSharper disable once UnusedType.Global
public static class FileSignatureExtensions
{
    private static readonly Dictionary<string, List<byte[]>> FileSignature = new()
    {
        {
            ".jpeg", new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
            }
        },
        {
            ".jpg", new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }
            }
        },
        {
            ".png", new List<byte[]>
            {
                new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                new byte[] { 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D } // PNG screenshot files on mac
            }
        }
    };

    public static string GetMimeType(this IEnumerable<byte> byteArray)
    {
        return byteArray.IsJpeg() ? "image/jpeg" : string.Empty;
    }

    private static bool IsJpeg(this IEnumerable<byte> byteArray)
    {
        var jpegSignatures = FileSignature[".jpeg"];
        var jpgSignatures = FileSignature[".jpg"];
        var headerBytes = byteArray.Take(jpegSignatures.Max(m => m.Length));
        return jpegSignatures.Any(signature =>
                   headerBytes.Take(signature.Length).SequenceEqual(signature))
               || jpgSignatures.Any(signature =>
                   headerBytes.Take(signature.Length).SequenceEqual(signature));
    }

    public static bool IsJpeg(this Stream stream)
    {
        return stream.ToByteArray().IsJpeg();
    }
}