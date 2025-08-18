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

using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core.Data.LowLevelProcesses;

public class SaveDataObjectRoutine: Routine
{
	private IRoutineProvider _routineProvider;
	
	public SaveDataObjectRoutine(IRoutineProvider rp)
	{
		_routineProvider = rp;
	}
	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		var dataObject = RoutineOperand as DataObject;
		
		if (dataObject == null)
		{
			throw new InvalidDataException("The DataRoutine operand is not an IDataObject");
		}

		if (dataObject.IsNew)
		{
			AddChildRoutine(_routineProvider.GetRoutine<InsertDataObjectRoutine>(dataObject), re);
		}
		else
		{
			AddChildRoutine(_routineProvider.GetRoutine<UpdateDataObjectRoutine>(dataObject), re);
		}
		
		// recursive walk hierarchy

		return null;
	}
}