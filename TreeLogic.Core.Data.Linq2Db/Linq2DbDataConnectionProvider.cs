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

using LinqToDB.Data;

namespace TreeLogic.Core.Data.Linq2Db;

public class Linq2DbDataConnectionProvider
{
	private readonly Func<DataConnection> _factory;
	
	public Linq2DbDataConnectionProvider(Func<DataConnection> factory)
	{
		_factory = factory;
	}

	public DataConnection GetConnection()
	{
		return _factory();
	}
}