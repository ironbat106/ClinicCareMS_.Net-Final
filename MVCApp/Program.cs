using BLL.Services;
using DAL.Repos;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<ClinicCareDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));



builder.Services.AddScoped<AppUserRepo>();
builder.Services.AddScoped<DoctorRepo>();
builder.Services.AddScoped<PatientRepo>();
builder.Services.AddScoped<AppointmentRepo>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<AppointmentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();