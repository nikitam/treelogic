using TreeLogic.Core;

namespace TreeLogic.Processing;

public class EchoRoutine: Routine
{
	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		Console.WriteLine("begin");

		for (var i = 0; i < 5; i++)
		{
			Thread.Sleep(4000);
			Console.WriteLine(i.ToString());
		}
		
		Console.WriteLine("end");
		
		return new StageRoutineResult()
		{
			Result = RoutineOperand
		};
	}
}