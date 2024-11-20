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

namespace TreeLogic.Core.Logging;

public interface ICoreLogger
{
	void Debug(string messageTemplate, params object[] values);
	
	void Warning(string messageTemplate, params object[] values);
	
	void Info(string messageTemplate, params object[] values);
	
	
	void Error(string messageTemplate, params object[] values);
	
	void Error(Exception exception, string messageTemplate, params object[] values);
	
	void Fatal(Exception exception, string messageTemplate, params object[] values);
}