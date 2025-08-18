using System.Threading.Channels;

namespace TreeLogic.Core.Services;

public class ProcessingChannelProvider
{
	private readonly Channel<RoutineProcessingItem> _channel;

	public ProcessingChannelProvider()
	{
		_channel = Channel.CreateUnbounded<RoutineProcessingItem>(new UnboundedChannelOptions
		{
			SingleReader = false,
			SingleWriter = false,
			AllowSynchronousContinuations = false
		});
	}
	
	internal Channel<RoutineProcessingItem> ProcessingChannel => _channel;
}