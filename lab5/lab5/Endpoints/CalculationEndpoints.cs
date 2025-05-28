using System.Globalization;
using Lab5.Services;

namespace Lab5.Endpoints
{
    public static class CalculationEndpoints
    {
        public static void MapCalculationEndpoints(this WebApplication app, CalculationService calculationService)
        {
            app.MapPost("/calculate", async context =>
            {
                var form = await context.Request.ReadFormAsync();

                if (form.Count == 0)
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<p>No input values found. Please go back and try again.</p>");
                    return;
                }

                string aString = form["A"];
                string bString = form["B"];
                string nString = form["N"];

                if (!double.TryParse(aString, NumberStyles.Any, CultureInfo.InvariantCulture, out double A) ||
                    !double.TryParse(bString, NumberStyles.Any, CultureInfo.InvariantCulture, out double B) ||
                    !int.TryParse(nString, out int n) || n < 1)
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<p>Invalid input values. Please go back and try again.</p>");
                    return;
                }

                double result;
                try
                {
                    result = calculationService.TrapezoidalIntegrator(calculationService.Function, A, B, n);
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync($"<p>Error: {ex.Message}</p>");
                    return;
                }

                var resultHtmlPath = Path.Combine(app.Environment.WebRootPath, "result.html");
                var resultHtml = await File.ReadAllTextAsync(resultHtmlPath);
                resultHtml = resultHtml.Replace("{{A}}", A.ToString())
                                       .Replace("{{B}}", B.ToString())
                                       .Replace("{{N}}", n.ToString())
                                       .Replace("{{RESULT}}", result.ToString());

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(resultHtml);
            });
        }
    }
}