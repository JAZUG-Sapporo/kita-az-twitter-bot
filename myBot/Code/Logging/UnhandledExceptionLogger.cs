using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace myBot
{
    public static class UnhandledExceptionLogger
    {
        public static void Write(Exception e)
        {
            var logText = new StringBuilder();
            logText.AppendLine("#exception");
            logText.AppendLine(e.ToString());

            var context = HttpContext.Current;
            var request = context != null ? context.Request : null;
            if (request != null)
            {
                try
                {
                    logText
                        .AppendLine()
                        .AppendLine("#request")
                        .AppendLine(request.HttpMethod + " " + request.RawUrl);
                    foreach (var key in request.Headers.AllKeys)
                    {
                        var value = key.ToLower().In("cookie") ? "****" : request.Headers[key];
                        logText.AppendLine(key + ": " + value);
                    }
                }
                catch { }

                try
                {
                    logText.AppendLine();
                    logText.AppendLine("#server-variables");
                    foreach (var key in request.ServerVariables.AllKeys.Except("ALL_HTTP", "ALL_RAW"))
                    {
                        var value = key.In("HTTP_COOKIE") ? "****" : request.ServerVariables[key];
                        logText.AppendLine(key + "=" + value);
                    }
                }
                catch { }
            }

            try
            {
                Trace.TraceError(logText.ToString());
                Trace.Flush();
            }
            catch { }
        }
    }
}