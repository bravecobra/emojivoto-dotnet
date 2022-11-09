using System.Diagnostics;
using System.Reflection;

namespace EmojiShared.Configuration;

public static class ActivitySourceFactory
{
    private static ActivitySource? _current;

    public static ActivitySource GetActivitySource()
    {
        return _current ??= new ActivitySource(Assembly.GetEntryAssembly()?.GetName().Name!);
    }
}