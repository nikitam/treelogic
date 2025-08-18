namespace TreeLogic.Core.Data;

public abstract class DataRoutine: TransactionalRoutine
{
	public const string TransactionTypeConst = "DB";
	protected void ValidateOperand()
	{
		
	}

	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		if (RoutineOperand is not DataObject)
		{
			throw new InvalidDataException("The DataRoutine operand is not an IDataObject");
		}

		return null;
	}

	public override string TransactionType
	{
		get
		{
			return DataRoutine.TransactionTypeConst;
		}
	}
}