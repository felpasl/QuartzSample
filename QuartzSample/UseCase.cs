using Serilog;

namespace QuartzSample;

public class UseCase : IUseCase
{
    private readonly ILogger logger;

    public UseCase(ILogger logger)
    {
        this.logger = logger;
    }
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        logger.Information("ScrapperJob is executing: " + DateTime.Now.ToString());    
    }
}