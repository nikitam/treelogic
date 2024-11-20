/*
   Copyright 2024 Nikita Mulyukin <nmulyukin@gmail.com>

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

using Serilog;

namespace TreeLogic.Core.Logging.Serilog;

public class CoreLogger: ICoreLogger
{
	private readonly ILogger _logger;
	internal CoreLogger(ILogger logger)
	{
		_logger = logger;
	}
	
	public void Debug(string messageTemplate, params object[] values)
	{
		_logger.Debug(messageTemplate, values);
	}

	public void Warning(string messageTemplate, params object[] values)
	{
		_logger.Warning(messageTemplate, values);
	}

	public void Info(string messageTemplate, params object[] values)
	{
		_logger.Information(messageTemplate, values);
	}

	public void Error(string messageTemplate, params object[] values)
	{
		_logger.Error(messageTemplate, values);
	}

	public void Error(Exception exception, string messageTemplate, params object[] values)
	{
		_logger.Error(exception, messageTemplate, values);
	}

	public void Fatal(Exception exception, string messageTemplate, params object[] values)
	{
		_logger.Fatal(exception, messageTemplate, values);
	}
}