using Lab5.Services;
using Lab5.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var calculationService = new CalculationService();
app.MapCalculationEndpoints(calculationService);

app.Run();