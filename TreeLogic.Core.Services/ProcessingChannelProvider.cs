using System.Threading.Channels;

namespace TreeLogic.Core.Services;

public class ProcessingChannelProvider
{
	private readonly Channel<Routine> _channel;

	public ProcessingChannelProvider()
	{
		_channel = Channel.CreateUnbounded<Routine>(new UnboundedChannelOptions
		{
			SingleReader = false,
			SingleWriter = false,
			AllowSynchronousContinuations = false
		});
	}
	
	internal Channel<Routine> ProcessingChannel => _channel;
}