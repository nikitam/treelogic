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

using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core;

public interface IRoutineManager
{
	RoutineResult Go<T>(object operand) where T : Routine;
	
	RoutineResult Go (Routine routine);
}
public class RoutineManager: IRoutineManager
{
	private ITransactionalRoutineComparerManager transactionalRoutineComparerManager;
	private ITransactionProviderManager _transactionProviderManager;
	private IRoutineProvider _routineProvider;

	public RoutineManager(IRoutineProvider rp, ITransactionProviderManager tpm, ITransactionalRoutineComparerManager trp)
	{
		_routineProvider = rp;
		_transactionProviderManager = tpm;
		transactionalRoutineComparerManager = trp;
	}

	public RoutineResult Go<T>(object operand) where T: Routine
	{
		return Go(_routineProvider.GetRoutine<T>(operand));
	}
	public RoutineResult Go(Routine routine)
	{
		var routineResult = new RoutineResult();
		var routineEnvironment = new RoutineEnvironment(routine);
		
		DoPrepare(routine, routineEnvironment, routineResult);
		
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
			var comparer = transactionalRoutineComparerManager.GetComparer(pair.Key);
			pair.Value.Sort(comparer);

			var transactionProvider = _transactionProviderManager.GetProvider(pair.Key);

			var transactionEnvironment = transactionProvider.CreateTransactionEnvironment();
			transactionEnvironment.OpenTransactionEnvironment();

			var transaction = transactionProvider.CreateTransaction(transactionEnvironment);
			
			transaction.Begin();
			transactionList.Add(transaction);

			foreach (var transactionalRoutine in pair.Value)
			{
				transactionalRoutine.Execute(transactionEnvironment);
			}
		}

		foreach (var transaction in transactionList)
		{
			transaction.Commit();
			
			transaction.Environment.CloseTransactionEnvironment();
			
			transaction.Dispose();
			transaction.Environment.Dispose();
		}
	}
}