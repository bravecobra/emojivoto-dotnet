using System.Diagnostics;
using System.Reflection;

namespace EmojiVoting.Configuration;

public static class ActivitySourceFactory
{
    public static ActivitySource CreateActivitySource()
    {
        return new ActivitySource(Assembly.GetEntryAssembly()?.GetName().Name!);
    }
}