using System.ComponentModel;
using System.Globalization;

namespace JitDalshe.Ui.Admin.Models;

[TypeConverter(typeof(GuidReferenceTypeConverter))]
public sealed class Ref<T> where T : struct
{
    public T Value { get; init; }

    public static implicit operator T(Ref<T> reference) => reference.Value;
}

public sealed class GuidReferenceTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is not string stringValue)
        {
            return base.ConvertFrom(context, culture, value);
        }

        return new Ref<Guid>
        {
            Value = Guid.Parse(stringValue)
        };
    }
}