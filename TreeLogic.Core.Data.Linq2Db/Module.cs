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

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using Microsoft.Extensions.DependencyInjection;
using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core.Data.Linq2Db;

public static class Module
{
	public static  IServiceCollection WithLinq2Db(this IServiceCollection serviceCollection, Func<DataConnection> factory)
	{
		var connectionProvider = new Linq2DbDataConnectionProvider(factory);
		serviceCollection.AddSingleton(connectionProvider);
		//serviceCollection.AddTransient<QueryDataObjectRoutine, Linq2DbQueryDataObjectRoutine>();
		
		var sp = serviceCollection.BuildServiceProvider();
		var tm = sp.GetService<ITransactionProviderManager>();
		tm.RegisterTransactionProvider(DataRoutine.TransactionTypeConst, new Linq2DbTransactionProvider(connectionProvider));
		
		return serviceCollection;
	}

}