namespace PerfumeMonitor.Web.Middleware
{
    public class CacheControlMiddleware
    {
        private readonly RequestDelegate _next;

        public CacheControlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Adicionar headers de cache control
            context.Response.OnStarting(() =>
            {
                var path = context.Request.Path.Value?.ToLower() ?? "";
                var headers = context.Response.Headers;

                if (path.StartsWith("/api/"))
                {
                    // APIs: sem cache
                    SetHeaderIfNotExists(headers, "Cache-Control", "no-cache, no-store, must-revalidate");
                    SetHeaderIfNotExists(headers, "Pragma", "no-cache");
                    SetHeaderIfNotExists(headers, "Expires", "0");
                }
                else if (path.EndsWith(".html") || path.EndsWith("/") || path == "")
                {
                    // HTML: cache mínimo
                    SetHeaderIfNotExists(headers, "Cache-Control", "no-cache, must-revalidate");
                    SetHeaderIfNotExists(headers, "Pragma", "no-cache");
                }
                else if (path.EndsWith(".css") || path.EndsWith(".js"))
                {
                    // CSS/JS: cache curto com validação
                    SetHeaderIfNotExists(headers, "Cache-Control", "public, max-age=300, must-revalidate");
                }
                else
                {
                    // Outros arquivos estáticos: cache médio
                    SetHeaderIfNotExists(headers, "Cache-Control", "public, max-age=3600");
                }

                // Adicionar ETag para validação
                var etag = $"\"{DateTime.UtcNow.Ticks}\"";
                SetHeaderIfNotExists(headers, "ETag", etag);

                return Task.CompletedTask;
            });

            await _next(context);
        }

        private static void SetHeaderIfNotExists(IHeaderDictionary headers, string key, string value)
        {
            if (!headers.ContainsKey(key))
            {
                headers[key] = value;
            }
        }
    }

    public static class CacheControlMiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheControl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CacheControlMiddleware>();
        }
    }
} 