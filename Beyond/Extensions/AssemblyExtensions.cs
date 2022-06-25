// ReSharper disable once CheckNamespace

namespace Beyond.Extensions.AssemblyExtended;

public static class AssemblyExtensions
{
    public static string GetManifestResourceAsString(this Assembly assembly, string resourceName)
    {
        var result = "";
        var resourceFileName = assembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase));

        if (string.IsNullOrEmpty(resourceFileName)) return result;

        using var stream = assembly.GetManifestResourceStream(resourceFileName);
        using var reader = new StreamReader(stream);
        result = reader.ReadToEnd();
        return result;
    }

    public static IEnumerable<Assembly> GetReferencedAssemblies(this Assembly assembly)
    {
        var listOfAssemblies = new List<Assembly>();
        listOfAssemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));
        return listOfAssemblies;
    }

    public static IEnumerable<Assembly> WithReferencedAssemblies(this Assembly assembly)
    {
        var listOfAssemblies = new List<Assembly>
        {
            assembly
        };
        listOfAssemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));
        return listOfAssemblies;
    }
}