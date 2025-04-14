namespace JitDalshe.Application.Errors;

public readonly struct Error
{
    public string Message { get; }
    public ErrorGroup Group { get; }

    private Error(string message, ErrorGroup group)
    {
        Message = message;
        Group = group;
    }

    public static Error Of(string message, ErrorGroup group = ErrorGroup.InternalError)
        => new(message, group);
}