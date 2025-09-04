using GetJob.Data;
using GetJob.ServiceContracts;
using GetJob.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<JobPortalContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJobService,JobService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();


builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
if (!app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapStaticAssets();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();


app.Run();
