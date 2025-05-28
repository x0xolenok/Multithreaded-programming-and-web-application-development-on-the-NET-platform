using lab6.Endpoints;
using lab6.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var sortingService = new SortingService();
app.MapSortEndpoint(sortingService);

app.Run();