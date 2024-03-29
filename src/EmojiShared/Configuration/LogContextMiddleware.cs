﻿using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace EmojiShared.Configuration
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Appends the TraceID to the response headers
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("TraceID", new StringValues(Activity.Current?.TraceId.ToString()));
            return _next(context);
        }
    }
}
