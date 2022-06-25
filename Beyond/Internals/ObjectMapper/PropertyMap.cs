// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Beyond.Internals.ObjectMapper;

internal class PropertyMap
{
    internal PropertyInfo? SourceProperty { get; set; }
    internal PropertyInfo? DestinationProperty { get; set; }
}