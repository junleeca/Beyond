// ReSharper disable UnusedMember.Global

namespace Beyond.Utilities;

// ReSharper disable once UnusedType.Global
public static class AssemblyUtility
{
    public static IEnumerable<Assembly>? GetEntryAssemblyWithReferences()
    {
        var listOfAssemblies = new List<Assembly>();
        var mainAsm = Assembly.GetEntryAssembly();

        if (mainAsm == null) return null;

        listOfAssemblies.Add(mainAsm);
        listOfAssemblies.AddRange(mainAsm.GetReferencedAssemblies().Select(Assembly.Load));
        return listOfAssemblies;
    }
}