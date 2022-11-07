using Fluxor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EmojiUI.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LoggingMiddleware : Fluxor.Middleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public override Task InitializeAsync(IDispatcher dispatcher, IStore store)
        {
            _logger.LogInformation(nameof(InitializeAsync));
            return Task.CompletedTask;
        }

        public override void AfterInitializeAllMiddlewares()
        {
            _logger.LogDebug(nameof(AfterInitializeAllMiddlewares));
        }

        public override bool MayDispatchAction(object action)
        {
            _logger.LogDebug(nameof(MayDispatchAction) + ObjectInfo(action));
            return true;
        }

        public override void BeforeDispatch(object action)
        {
            _logger.LogDebug(nameof(BeforeDispatch) + ObjectInfo(action));
        }

        public override void AfterDispatch(object action)
        {
            _logger.LogDebug(nameof(AfterDispatch) + ObjectInfo(action));
        }

        private string ObjectInfo(object obj)
            => ": " + obj.GetType().Name + " " + JsonConvert.SerializeObject(obj);
    }
}
