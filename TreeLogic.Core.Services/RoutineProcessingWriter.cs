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

namespace TreeLogic.Core.Services;

public class RoutineProcessingWriter
{
	private readonly ProcessingChannelProvider _channelProvider;

	public RoutineProcessingWriter(ProcessingChannelProvider pcp)
	{
		_channelProvider = pcp;
	}
	
	public void WriteRoutine(Routine routine)
	{
		_channelProvider.ProcessingChannel.Writer.TryWrite(new RoutineProcessingItem
		{
			Routine = routine
		});
	}

	public Task WriteRoutineAsync(Routine routine, Func<StageRoutineResult, Task> code)
	{
		var rpi = new RoutineProcessingItem
		{
			Routine = routine,
			Code = code,
			TaskCompletionSource = new TaskCompletionSource<bool>()
		};
		
		_channelProvider.ProcessingChannel.Writer.TryWrite(rpi);
		return rpi.TaskCompletionSource.Task;
	}
}