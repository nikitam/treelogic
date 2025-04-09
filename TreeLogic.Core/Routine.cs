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

namespace TreeLogic.Core;

public abstract class Routine
{
	private List<Routine> _childRoutines;

	public Routine()
	{
		_childRoutines = new List<Routine>();
	}

	public Routine ParentRoutine { get; set; }

	public IList<Routine> ChildRoutines
	{
		get
		{
			return _childRoutines;
		}
	}

	public void AddChildRoutine(Routine routine, RoutineEnvironment re)
	{
		_childRoutines.Add(routine);
		routine.ParentRoutine = this;

		if (routine is TransactionalRoutine transactionalRoutine)
		{
			re.AddTransactionalRoutine(transactionalRoutine);
		}
	}

	public abstract StageRoutineResult Prepare(RoutineEnvironment re);

	public object RoutineOperand { get; internal set; }
	
	
	public TaskCompletionSource<bool> Tcs { get; set; }
	public Func<StageRoutineResult, Task> Code { get; set; }
}