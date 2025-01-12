/*
   Copyright 2025 Nikita Mulyukin <nmulyukin@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Microsoft.Extensions.Hosting;

namespace TreeLogic.Core.Services;

public class RoutineProcessingService: BackgroundService
{
	private readonly RoutineProcessingServiceOptions _options;
	private readonly ProcessingChannelProvider _channelProvider;
	private readonly IRoutineManager _routineManager;

	public RoutineProcessingService(RoutineProcessingServiceOptions options, 
		ProcessingChannelProvider cp, IRoutineManager rm)
	{
		_options = options;
		_channelProvider = cp;
		_routineManager = rm;
	}
	
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var readingTasks = new List<Task>();
		
		for (var i = 0; i < _options.ThreadCount; i++)
		{
			var readingTask = new Task(ReadAsync, TaskCreationOptions.LongRunning, stoppingToken);
			readingTasks.Add(readingTask);
			readingTask.Start();
		}
		
		await Task.WhenAll(readingTasks);
	}

	private async void ReadAsync(object o)
	{
		var cc = (CancellationToken)o;
		while (await _channelProvider.ProcessingChannel.Reader.WaitToReadAsync(cc))
		{
			while (_channelProvider.ProcessingChannel.Reader.TryRead(out var routine))
			{
				_routineManager.Go(routine);
			}
		}
	}
}