using System.Globalization;
using lab6.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace lab6.Endpoints
{
    public static class SortEndpoint
    {
        public static void MapSortEndpoint(this IEndpointRouteBuilder app, SortingService sortingService)
        {
            app.MapPost("/sort", async context =>
            {
                var form = await context.Request.ReadFormAsync();
                var numbersInput = form["numbers"].ToString();

                float[] numbers;
                try
                {
                    numbers = numbersInput.Split(',')
                        .Select(n => float.Parse(n.Trim(), CultureInfo.InvariantCulture))
                        .ToArray();
                }
                catch
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<p>Invalid input. Please enter numbers separated by commas.</p>");
                    return;
                }

                // Get sorted float array from the service.
                var sortedArray = sortingService.BubbleSort(numbers);

                // Join the sorted numbers into a string.
                var sortedNumbers = string.Join(", ", sortedArray.Select(n => n.ToString(CultureInfo.InvariantCulture)));

                // Debug log to verify the sorted numbers.
                Console.WriteLine("Sorted numbers: " + sortedNumbers);

                var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
                var resultHtmlPath = Path.Combine(env.WebRootPath, "result.html");
                var resultHtml = await File.ReadAllTextAsync(resultHtmlPath);

                resultHtml = resultHtml.Replace("{{INPUT}}", numbersInput)
                    .Replace("{{SORTED}}", sortedNumbers);

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(resultHtml);
            });
        }
    }
}