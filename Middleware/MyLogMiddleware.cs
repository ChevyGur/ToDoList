
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

            string logMessage = $"{act} started at: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
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
