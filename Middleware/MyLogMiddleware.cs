// using System.Diagnostics;

// namespace MyMiddleware{
// public class MyLogMiddleware
//     {
//         private readonly RequestDelegate next;
//         private readonly ILogger<MyLogMiddleware> logger;

//         public MyLogMiddleware(RequestDelegate next, ILogger<MyLogMiddleware> logger)
//         {
//             this.next = next;
//             this.logger = logger;
//         }

//         public async Task Invoke(HttpContext c)
//         {
//             var act = $"{c.Request.Path}.{c.Request.Method}";
//             var sw = new Stopwatch();
//             sw.Start();

//             logger.LogInformation($"{act} started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
//             await next.Invoke(c);
//             logger.LogDebug($"{act} ended at {sw.ElapsedMilliseconds} ms."
//             + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
//         }
//     }

//     public static partial class MiddlewareExtensions
//     {
//         public static IApplicationBuilder UseMyLogMiddleware(this IApplicationBuilder builder)
//         {
//             return builder.UseMiddleware<MyLogMiddleware>();
//         }
//     }
// }



using System.Diagnostics;

namespace MyMiddleware
{
    public class MyLogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string logFilePath;

        public MyLogMiddleware(RequestDelegate next, string logFilePath)
        {
            this.next = next;
            this.logFilePath = logFilePath;
        }

        public async Task Invoke(HttpContext c)
        {
            var act = $"{c.Request.Path}.{c.Request.Method}";
            var sw = new Stopwatch();
            sw.Start();

            string logMessage = $"{act} started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
            WriteLogToFile(logMessage);

            await next.Invoke(c);

            logMessage = $"{act} ended at {sw.ElapsedMilliseconds} ms. UserId: {c.User?.FindFirst("Id")?.Value ?? "unknown"}";
            WriteLogToFile(logMessage);
        }

        private void WriteLogToFile(string logMessage)
        {
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(logMessage);
            }
        }
    }

    public static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMyLogMiddleware(this IApplicationBuilder builder, string logFilePath)
        {
            return builder.UseMiddleware<MyLogMiddleware>(logFilePath);
        }
    }
}
