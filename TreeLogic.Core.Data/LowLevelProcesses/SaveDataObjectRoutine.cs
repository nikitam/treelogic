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
		var dataObject = RoutineOperand as IDataObject;
		
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

		return null;
	}
}