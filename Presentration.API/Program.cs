using Apllication;
using Apllication.Common.Interfaces;
using Infrastructre;
using Presentration.API.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Companies", builder =>
        builder.Expire(TimeSpan.FromMinutes(5))
        .Tag("companies"));

    options.AddPolicy("Games", builder =>
        builder.Expire(TimeSpan.FromMinutes(5))
        .Tag("games"));

    options.AddPolicy("Users", builder =>
        builder.Expire(TimeSpan.FromMinutes(5))
        .Tag("Users"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
