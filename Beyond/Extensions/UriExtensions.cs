// ReSharper disable once CheckNamespace
// ReSharper disable UnusedMember.Global
namespace Beyond.Extensions.UriExtensions;

// ReSharper disable once UnusedType.Global
public static class UriExtensions
{
    public static string ReadAsString(this Uri uri)
    {
        var httpClient = new HttpClient();

        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
        var content = response.Content.ReadAsStream();
        var reader = new StreamReader(content);
        var text = reader.ReadToEnd();

        return text;
    }

    public static Stream ReadAsStream(this Uri uri)
    {
        var httpClient = new HttpClient();

        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
        var content = response.Content.ReadAsStream();

        return content;
    }

    public static byte[] ReadAsBytesArray(this Uri uri)
    {
        var httpClient = new HttpClient();

        var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
        var content = response.Content.ReadAsStream();

        using var memoryStream = new MemoryStream();
        content.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }

    public static async Task<byte[]> ReadAsBytesArrayAsync(this Uri uri, CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();

        var response = await httpClient.GetAsync(uri, cancellationToken);
        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        return content;
    }

    public static async Task<Stream> ReadAsStreamAsync(this Uri uri, CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();

        var response = await httpClient.GetAsync(uri, cancellationToken);
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);

        return content;
    }

    public static async Task<string> ReadAsStringAsync(this Uri uri, CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();

        var response = await httpClient.GetAsync(uri, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }
}