using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using CompanyEmployees;
using CompanyEmployees.Extensions;
using Contracts;
using ActionFilters;



var builder = WebApplication.CreateBuilder(args);

LogManager.Setup()
    .LoadConfigurationFromFile(
    string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

// Suppress ApiController attribute errors to allow custom error messages
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Register action filters
builder.Services.AddScoped<ValidationFilterAttribute>();


builder.Services.AddControllers(config =>
{
    // Add Json formatter configuration for PATCH requests
    config.InputFormatters.Insert(0, JsonPatchConfiguration.GetJsonPatchInputFormatter());

    // content negotiation
    config.RespectBrowserAcceptHeader = true; // respect Accept header from client

    config.ReturnHttpNotAcceptable = true; // 406 response for unsupported media types

}).AddXmlDataContractSerializerFormatters() // support xml formatter
    .AddCustomCsvFormatter()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);



/// <summary>
/// Application builder object
/// </summary>
var app = builder.Build();

// Add global exception handler manager 
var logger = app.Services.GetRequiredService<ILoggerManager>();

app.ConfigureExceptionHandler(logger);


if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();


app.MapControllers();

app.Run();
