var builder = WebApplication.CreateBuilder(args);

// Registrar los servicios de Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseRequestLocalization("en-US");

app.MapStaticAssets();
app.MapRazorPages();


app.Run();