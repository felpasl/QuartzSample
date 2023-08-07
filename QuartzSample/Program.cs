
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System.Collections.Specialized;
using Autofac.Extras.Quartz;
using QuartzSample;
using Quartz;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

await new HostBuilder()
    .UseSerilog()
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureServices(ConfigureServices)
    .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
    .RunConsoleAsync();


static void ConfigureServices(IServiceCollection services)
{
    // Use extensions from libraries to register services in the
    // collection. These will be automatically added to the Autofac
    // container.
    services.AddSerilog();
    services.AddQuartz(configure => {
        configure.UsePersistentStore(persistence => {
            persistence.UseNewtonsoftJsonSerializer();
            persistence.UsePostgres("Server=db;Port=5432;Database=postgres;User Id=postgres;Password=postgres;");  
            persistence.SetProperty("quartz.jobStore.tablePrefix", "quartz.qrtz_");       
            persistence.UseProperties = true;
        });
        
        configure.SchedulerId = "QuartzSample";
        configure.SchedulerName = "QuartzSample";
        configure.InterruptJobsOnShutdown = true;
        configure.InterruptJobsOnShutdownWithWait = true;
        configure.BatchTriggerAcquisitionFireAheadTimeWindow = TimeSpan.Zero;

        // Create a "key" for the job
        var jobName = "ScrapperJob";
        var jobKey = new JobKey(jobName);

                // Register the job with the DI container
        configure.AddJob<ScrapperJob>(opts => opts.WithIdentity(jobKey));
                
                // Create a trigger for the job
        configure.AddTrigger(opts => opts
            .ForJob(jobKey) // link to the HelloWorldJob
            .WithIdentity($"{jobName}-trigger") // give the trigger a unique name
            .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds
    });
    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
}

static void ConfigureContainer(ContainerBuilder builder)
{
    // Add any Autofac modules or registrations. This is called AFTER
    // ConfigureServices so things you register here OVERRIDE things
    // registered in ConfigureServices.
    //
    // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
    // when building the host or this won't be called.
    builder.RegisterInstance<ILogger>(Log.Logger);

    builder.RegisterType<UseCase>().As<IUseCase>();

}