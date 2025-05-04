namespace JitDalshe.Ui.Admin.Models;

public class GuidReference
{
    public Guid Value { get; set; }

    public static implicit operator Guid(GuidReference reference) => reference.Value;
}