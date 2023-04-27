using Apllication;
using Domain.Entities.Companies;
using Domain.Entities.Genres;
using Domain.Entities.Platforms;
using Domain.Entities.Users;
using Domain.Helpers;
using Infrastructre;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Presentration.API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<UserId>());
        options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<CompanyId>());
        options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<GenreId>());
        options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<PlatformId>());
        options.SerializerSettings.Converters.Add(new StronglyTypedIdJsonConverter<GenreId>());
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAPI(builder.Configuration);

var app = builder.Build();

app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
