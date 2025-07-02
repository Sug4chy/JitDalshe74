namespace JitDalshe.Ui.Admin.Services.Shared;

public sealed class Runner
{
    private Action<Exception> _errorCallback = null!;

    public void ConfigureErrorCallback(Action<Exception> callback)
    {
        _errorCallback = callback;
    }

    public async Task RunCatchingAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception e)
        {
            _errorCallback?.Invoke(e);
        }
    }

    public async Task<T> RunCatchingAsync<T>(Func<Task<T>> action, T defaultValue = default!)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            _errorCallback?.Invoke(e);
            return defaultValue;
        }
    }
}