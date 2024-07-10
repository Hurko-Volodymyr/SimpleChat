//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
//using System;
//using System.Net;
//using System.Threading.Tasks;

//namespace SimpleChat
//{
//    public class ErrorHandlingMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public ErrorHandlingMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception ex)
//            {
//                await HandleExceptionAsync(context, ex);
//            }
//        }

//        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
//        {
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
//            {
//                StatusCode = context.Response.StatusCode,
//                Message = "Internal Server Error."
//            }));
//        }
//    }
//}
