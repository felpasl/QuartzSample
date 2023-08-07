using Quartz;
using Serilog;

namespace QuartzSample;
[PersistJobDataAfterExecution]
[DisallowConcurrentExecution]
public class ScrapperJob : IJob
{
    private readonly ILogger logger;
    private readonly IUseCase useCase;

    public ScrapperJob(ILogger logger, IUseCase useCase)
    {
        this.logger = logger;
        this.useCase = useCase;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        logger.Information($"Last Run: {context.JobDetail.JobDataMap.GetString("LastRun")}");  
        await useCase.Execute();
        context.JobDetail.JobDataMap.Put("LastRun", DateTime.Now.ToString()); 
    }
}
