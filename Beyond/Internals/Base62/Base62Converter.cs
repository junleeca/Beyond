using Beyond.Enums;
// ReSharper disable UnusedMember.Global

namespace Beyond.Internals.Base62;

// ReSharper disable once UnusedType.Global
internal class Base62Converter
{
    private const string DefaultCharacterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string InvertedCharacterSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly string characterSet;

    internal Base62Converter()
    {
        characterSet = DefaultCharacterSet;
    }

    internal Base62Converter(Base62CharacterSet charset)
    {
        characterSet = charset == Base62CharacterSet.Default ? DefaultCharacterSet : InvertedCharacterSet;
    }

    internal string Encode(string value)
    {
        var arr = new int[value.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = value[i];
        }

        return Encode(arr);
    }

    internal string Decode(string value)
    {
        var arr = new int[value.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = characterSet.IndexOf(value[i]);
        }

        return Decode(arr);
    }

    private string Encode(int[] value)
    {
        var converted = BaseConvert(value, 256, 62);
        var builder = new StringBuilder();
        foreach (var t in converted)
        {
            builder.Append(characterSet[t]);
        }

        return builder.ToString();
    }

    private static string Decode(int[] value)
    {
        var converted = BaseConvert(value, 62, 256);
        var builder = new StringBuilder();
        foreach (var t in converted)
        {
            builder.Append((char)t);
        }

        return builder.ToString();
    }

    private static int[] BaseConvert(int[] source, int sourceBase, int targetBase)
    {
        var result = new List<int>();
        int count;
        while ((count = source.Length) > 0)
        {
            var quotient = new List<int>();
            var remainder = 0;
            for (var i = 0; i != count; i++)
            {
                var accumulator = source[i] + remainder * sourceBase;
                var digit = accumulator / targetBase;
                remainder = accumulator % targetBase;
                if (quotient.Count > 0 || digit > 0)
                {
                    quotient.Add(digit);
                }
            }

            result.Insert(0, remainder);
            source = quotient.ToArray();
        }

        return result.ToArray();
    }
}