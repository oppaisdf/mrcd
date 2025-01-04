using api.Common;
using api.Context;
using api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string defaultConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")!;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

EncryptionConverter.InitializeKey(Environment.GetEnvironmentVariable("ENCRYPT_KEY")!);

builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped(typeof(INameService<>), typeof(NameService<>));
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPublicService, PublicService>();

builder.Services.AddDbContext<MerContext>((provider, options) =>
{
    options.UseMySql(
        defaultConnection,
        ServerVersion.AutoDetect(defaultConnection));
});

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MerContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "mrcd-session";
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.LoginPath = "/api/Login";
    options.AccessDeniedPath = "/api/Login/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
