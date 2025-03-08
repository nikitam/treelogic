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

using Microsoft.Extensions.DependencyInjection;
using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core;

public static class Module
{
	public static IServiceCollection AddTreeLogic(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<IRoutineProvider, RoutineProvider>();
		serviceCollection.AddSingleton<ITransactionProviderManager>(new TransactionProviderManager());
		serviceCollection.AddSingleton<ITransactionalRoutineComparerManager>(new TransactionalRoutineComparerManager());

		serviceCollection.AddSingleton<IRoutineManager>(sp =>
		{
			var routineProvider = sp.GetRequiredService<IRoutineProvider>();
			var transactionProviderManager = sp.GetRequiredService<ITransactionProviderManager>();
			var transactionRoutineComparerProvider = sp.GetRequiredService<ITransactionalRoutineComparerManager>();
			return new RoutineManager(routineProvider, transactionProviderManager, transactionRoutineComparerProvider);
		});

		return serviceCollection;
	}
}