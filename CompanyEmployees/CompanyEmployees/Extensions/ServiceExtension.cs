using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;
using Service;
using Service.Contracts;

namespace CompanyEmployees.Extensions;

public static class ServiceExtension
{
	/// <summary>
	/// Configure cors for application
	/// </summary>
	/// <param name="services"></param>
	public static void ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", builder => 
				builder.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());
		});
	}

	public static void ConfigureIISIntegration(this IServiceCollection services) =>
		services.Configure<IISOptions>(options =>
		{

		});

	/// <summary>
	/// <b>Logger Service Dependency Injection Configuration</b>
	/// </summary>
	/// <param name="services"></param>
	public static void ConfigureLoggerService(this IServiceCollection services) =>
		services.AddSingleton<ILoggerManager, LoggerManager>();

	/// <summary>
	/// Configure the RepositoryManager class and IRepositoryManager interface
	/// dependency injection
	/// </summary>
	/// <param name="services"></param>
	public static void ConfigureRepositoryManager(this IServiceCollection services) =>
		services.AddScoped<IRepositoryManager, RepositoryManager>();

    /// <summary>
    /// Configure the ServiceManager class and IServiceManager interface
    /// dependency injection
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureServiceManager(this IServiceCollection services) =>
		services.AddScoped<IServiceManager, ServiceManager>();

	/// <summary>
	/// Register runtime RepositoryContext
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configuration"></param>
	public static void ConfigureSqlContext(this IServiceCollection services,
		IConfiguration configuration) =>
		services.AddDbContext<RepositoryContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

    /// <summary>
    /// <b>Adds a custom csv output formatter to the application</b>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
		builder.AddMvcOptions(config => config.OutputFormatters.Add(
			new CsvOutputFormatter()));


}