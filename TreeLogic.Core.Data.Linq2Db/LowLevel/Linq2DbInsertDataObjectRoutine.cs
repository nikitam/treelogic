using LinqToDB;
using TreeLogic.Core.Abstractions;
using TreeLogic.Core.Data.LowLevelProcesses;

namespace TreeLogic.Core.Data.Linq2Db.LowLevel;

public class Linq2DbInsertDataObjectRoutine: InsertDataObjectRoutine
{
	public override void Execute(ITransactionEnvironment environment)
	{
		var linq2dbEnv = environment as Linq2DbTransactionEnvironment;
		linq2dbEnv.Insert(RoutineOperand);
	}
}