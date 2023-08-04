using Quartz;
using Serilog;

namespace QuartzSample;
[PersistJobDataAfterExecution]
[DisallowConcurrentExecution]
public class ScrapperJob : IJob
{
    private readonly ILogger logger;

    public ScrapperJob(ILogger logger)
    {
        this.logger = logger;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        logger.Information($"Last Run: {context.JobDetail.JobDataMap.GetString("LastRun")}");
        logger.Information("ScrapperJob is executing: " + context.JobDetail.Key.Name);       
        context.JobDetail.JobDataMap.Put("LastRun", DateTime.Now.ToString()); 
    }
}
