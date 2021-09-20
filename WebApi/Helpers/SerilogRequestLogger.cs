using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Context;
using WebApi.Models;

namespace WebApi.Helpers
{
    public class SerilogRequestLogger
    {
        readonly RequestDelegate _next;

        public SerilogRequestLogger(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            // Push the user name into the log context so that it is included in all log entries

            // Getting the request body is a little tricky because it's a stream
            // So, we need to read the stream and then rewind it back to the beginning
            string requestBody = "";
            httpContext.Request.EnableBuffering();
            Stream body = httpContext.Request.Body;
            byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
            await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            httpContext.Request.Body = body;

            httpContext.Request.Headers.TryGetValue("Correlation-Id-Header", out var corelationIds);

            var corelationId = corelationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

            using (LogContext.PushProperty("CorelationId", corelationId))
            {
                string sessionId = string.Empty;
                if (httpContext.Request.Headers["sessionid"].SingleOrDefault() != null)
                {
                    sessionId = httpContext.Request.Headers["sessionid"].SingleOrDefault();
                    LogContext.PushProperty("SessionId", httpContext.Request.Headers["sessionid"].SingleOrDefault());
                }
                httpContext.Request.Headers.Add("CorelationId", corelationId);

                Log.ForContext("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
               .ForContext("RequestBody", requestBody)
               .Information("Request information {RequestMethod} {RequestPath} information", httpContext.Request.Method, httpContext.Request.Path);


                // The reponse body is also a stream so we need to:
                // - hold a reference to the original response body stream
                // - re-point the response body to a new memory stream
                // - read the response body after the request is handled into our memory stream
                // - copy the response in the memory stream out to the original response stream
                using (var responseBodyMemoryStream = new MemoryStream())
                {
                    var originalResponseBodyReference = httpContext.Response.Body;
                    httpContext.Response.Body = responseBodyMemoryStream;
                    //if (!httpContext.Response.Headers.ContainsKey("ResponseId"))
                    //{
                    //    httpContext.Response.Headers.Add("ResponseId", corelationId);
                    //}
                    //else
                    //{
                    //    httpContext.Response.Headers["ResponseId"] = corelationId;
                    //}
                    try
                    {
                        await _next.Invoke(httpContext);
                    }
                    catch (Exception exception)
                    {
                        Log.ForContext("Type", "Error")
                                 .ForContext("Exception", exception, destructureObjects: true)
                                 .Error(exception, exception.Message + ". {@errorId}", corelationId);
                        await HandleExceptionAsync(httpContext, exception, corelationId);
                    }
                    if (!httpContext.Response.HasStarted)
                    {

                    }

                    httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
                    httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                    Log.ForContext("ResponseBody", responseBody)
                       .Information("Response information {RequestMethod} {RequestPath} {statusCode}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode);

                    await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
                }
            }
        }


        private Task HandleExceptionAsync(HttpContext context, Exception exception, string corelationId)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            context.Response.ContentType = "application/json";
            {
                context.Response.StatusCode = 500;
                var response = new BaseCustomException(
                        exception.Message,
                        exception.StackTrace,
                        500,
                        corelationId
                       );
                return context.Response.WriteAsync(JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver,
                        Formatting = Formatting.Indented
                    }
                    ));
            }

        }
    }
}
