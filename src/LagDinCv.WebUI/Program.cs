using LagDinCv.Infrastructure;
using LagDinCv.WebUI;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddRazorPages();
builder.Services.AddPdfBuilder();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// adds Resume-folder to path so it can be displayed frontend 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider( Path.Combine(Directory.GetCurrentDirectory(), "Resumes")), 
    RequestPath = "/Resumes" 
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();