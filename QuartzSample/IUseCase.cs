namespace QuartzSample;

public interface IUseCase
{
    Task Execute(CancellationToken cancellationToken = default);
}