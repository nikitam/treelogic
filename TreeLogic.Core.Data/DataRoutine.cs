namespace TreeLogic.Core.Data;

public abstract class DataRoutine: TransactionalRoutine
{
	protected void ValidateOperand()
	{
		
	}

	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		if (RoutineOperand is not IDataObject)
		{
			throw new InvalidDataException("The DataRoutine operand is not an IDataObject");
		}

		return null;
	}

	public override string TransactionType
	{
		get
		{
			return "DB";
		}
	}
}