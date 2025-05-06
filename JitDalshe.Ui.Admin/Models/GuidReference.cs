using System.ComponentModel;
using System.Globalization;

namespace JitDalshe.Ui.Admin.Models;

[TypeConverter(typeof(GuidReferenceTypeConverter))]
public sealed class GuidReference
{
    public Guid Value { get; set; }

    public static implicit operator Guid(GuidReference reference) => reference.Value;
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

        return new GuidReference
        {
            Value = Guid.Parse(stringValue)
        };
    }
}