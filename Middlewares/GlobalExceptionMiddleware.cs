using System.Net;
using System.Text.Json;

namespace FitnessTrackerPAW.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Tratare pentru rute inexistente (404) sau acces interzis (403)
                // care nu arunca propriu-zis exceptii in pipeline
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleExceptionAsync(context, new Exception("Resursa solicitata nu a fost gasita."), HttpStatusCode.NotFound);
                }
                else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await HandleExceptionAsync(context, new Exception("Nu ai permisiunea de a accesa aceasta resursa."), HttpStatusCode.Forbidden);
                }
            }
            catch (ArgumentException ex) // Folosit de obicei pentru erori de validare - 400 Bad Request
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedAccessException ex) // 403 Forbidden
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
            }
            catch (Exception ex) // Orice alta eroare neprevazuta - 500 Internal Server Error
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}