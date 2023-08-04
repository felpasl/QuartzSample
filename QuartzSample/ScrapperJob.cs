using Quartz;
using Serilog;

namespace QuartzSample;
public class ScrapperJob : IJob
{
    private readonly ILogger logger;

    public ScrapperJob(ILogger logger)
    {
        this.logger = logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        logger.Information("ScrapperJob is executing: " + context.JobDetail.Key.Name);        
    }
}
