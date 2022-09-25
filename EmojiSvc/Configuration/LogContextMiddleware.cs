using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EmojiSvc.Configuration
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            //Append the TraceID to the response headers
            context.Response.Headers.Add("TraceID", new StringValues(Activity.Current?.TraceId.ToString()));
            return _next(context);
        }
    }
}
