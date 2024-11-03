var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(); // <-- This is the key

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapDefaultControllerRoute();

app.Run();
