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

public interface IRoutineManager
{
	RoutineResult Go<T>(object operand) where T : Routine;
}
public class RoutineManager: IRoutineManager
{
	private ITransactionRoutineComparerProvider _transactionRoutineComparerProvider;
	private ITransactionProvider _transactionProvider;
	private IRoutineProvider _routineProvider;

	public RoutineManager(IRoutineProvider rp)
	{
		_routineProvider = rp;
	}

	public RoutineResult Go<T>(object operand) where T: Routine
	{
		return InternalGo(_routineProvider.GetRoutine<T>(operand));
	}
	internal RoutineResult InternalGo(Routine routine)
	{
		var routineResult = new RoutineResult();
		var routineEnvironment = new RoutineEnvironment(routine);
		
		// привнесение дополнительной функциональности через модель middleware asp.net
		
		// Prepare Phase
		DoPrepare(routine, routineEnvironment, routineResult);
		
		// Execute Phase
		DoExecute(routineEnvironment, routineResult);

		return routineResult;
	}

	private void DoPrepare(Routine routine, RoutineEnvironment re, RoutineResult rr)
	{
		var result = routine.Prepare(re);
		if (rr != null)
		{
			rr.PrepareResult = result;
		}

		foreach (var childRoutine in routine.ChildRoutines)
		{
			DoPrepare(childRoutine, re, null);
		}
	}

	private void DoExecute(RoutineEnvironment re, RoutineResult rr)
	{
		var transactionList = new List<ITransaction>();
		
		// return as result transactions statistic
		
		foreach (var pair in re.TransactionalRoutines)
		{
			var comparer = _transactionRoutineComparerProvider.GetComparer(pair.Key);
			pair.Value.Sort(comparer);

			var transaction = _transactionProvider.CreateTransaction(pair.Key);
			
			transaction.Begin();
			transactionList.Add(transaction);

			foreach (var transactionalRoutine in pair.Value)
			{
				transactionalRoutine.Execute();
			}
		}

		foreach (var transaction in transactionList)
		{
			transaction.Commit();
		}
	}
}