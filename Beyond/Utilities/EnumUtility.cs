// ReSharper disable UnusedMember.Global

using Beyond.Extensions.EnumExtended;

namespace Beyond.Utilities;

// ReSharper disable once UnusedType.Global
public static class EnumUtility
{
    public static bool ContainsName<TEnum>(string? name, bool ignoreCase = false) where TEnum : Enum
    {
        if (name == null) return false;
        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetNames<TEnum>().Any(item => item.Contains(name, stringComparison));
    }

    public static bool ContainsValue<TEnum>(string? value, bool ignoreCase = false) where TEnum : Enum
    {
        if (value == null) return false;
        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetValuesAsString<TEnum>().Any(item => item.Contains(value, stringComparison));
    }

    public static bool ContainsValue<TEnum>(TEnum value) where TEnum : Enum
    {
        return GetValues<TEnum>().Contains(value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<string> GetDescriptions<TEnum>(bool replaceNullWithEnumName = false) where TEnum : Enum
    {
        return GetValues<TEnum>().Select(e => e.GetDescription(replaceNullWithEnumName)).Where(x => x != null)!;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<string> GetNames<TEnum>() where TEnum : Enum
    {
        return Enum.GetNames(typeof(TEnum));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum
    {
        return (TEnum[])Enum.GetValues(typeof(TEnum));
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<string> GetValuesAsString<TEnum>() where TEnum : Enum
    {
        return GetValues<TEnum>().Select(e => e.ToString());
    }

    public static bool IsDefined<TEnum>(this string name) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), name);
    }

    public static bool IsDefined<TEnum>(this TEnum value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }

    public static bool IsInEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
    {
        var enums = GetValuesAsString<TEnum>().Select(e => ignoreCase ? e.ToLower() : e);
        return enums.Contains(ignoreCase ? value.ToLower() : value);
    }

    public static bool IsInEnumDescription<TEnum>(this string value, bool ignoreCase = false) where TEnum : Enum
    {
        var enums = GetDescriptions<TEnum>().Select(e => ignoreCase ? e.ToLower() : e);
        return enums.Contains(ignoreCase ? value.ToLower() : value);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IEnumerable<EnumMemberInfo<T>> GetEnumInfo<T>() where T : Enum
    {
        var result = new List<EnumMemberInfo<T>>();
        var names = Enum.GetNames(typeof(T));
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var name in names)
        {
            var parsed = Enum.Parse(typeof(T), name);
            var item = (T)parsed;
            var value = Convert.ToInt32(parsed);
            var description = item.GetDescription(true);
            result.Add(new EnumMemberInfo<T>
            {
                Name = name,
                Value = value,
                Description = description,
                Item = item
            });
        }

        return result;
    }

    public static IEnumerable<EnumMemberInfo<T>> GetEnumInfo<T>(Func<EnumMemberInfo<T>, bool> predicate) where T : Enum
    {
        var result = GetEnumInfo<T>().Where(predicate);
        return result;
    }
}