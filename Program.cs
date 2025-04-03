using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskApp.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Database Context (Ensure Connection String is Set in appsettings.json)
builder.Services.AddDbContext<TaskAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskAppContext")
    ?? throw new InvalidOperationException("Connection string 'TaskAppContext' not found.")));

// ✅ Add MVC Support
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// ✅ Ensure Routes Work
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}"
);

app.Run();