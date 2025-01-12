namespace TreeLogic.Processing;

public class NullHostedService: BackgroundService
{
	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Console.WriteLine("Null hosted service is starting.");
		return Task.CompletedTask;
	}
}