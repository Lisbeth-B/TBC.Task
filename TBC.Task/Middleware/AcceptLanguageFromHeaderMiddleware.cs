using System.Globalization;

namespace TBC.Task.Api.Middleware
{
    public class AcceptLanguageFromHeaderMiddleware : IMiddleware
    {
        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? language;
            try
            {
                language = context.Request.Headers.AcceptLanguage.ToString().Split(',').FirstOrDefault();
            }
            catch
            {
                language = "en-US";
            }

            CultureInfo.CurrentCulture = new CultureInfo(language);
            CultureInfo.CurrentUICulture = new CultureInfo(language);

            await next(context);
        }
    }
}
