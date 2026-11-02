
using NoteApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Read MongoDB connection string from appsettings.json or environment variable
string connectionString =
    builder.Configuration["MongoDB:ConnectionString"]
    ?? throw new Exception("MongoDB connection string is missing!");

builder.Services.Configure<DatabaseSettings>(options =>
{
    options.ConnectionString = connectionString;
    options.DatabaseName = "NoteDb";
});

builder.Services.AddSingleton<NoteService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notes}/{action=Index}/{id?}");

Console.WriteLine("NoteApp running on http://0.0.0.0:5050");

app.Run("http://0.0.0.0:5050");
