using TreeLogic.Core;
using TreeLogic.Core.Abstractions;

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

public class ComplexRoutine : Routine
{
	IRoutineProvider _provider;

	public ComplexRoutine(IRoutineProvider p)
	{
		_provider = p;
	}
	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		Console.WriteLine("ComplexRoutine");
		
		var firstChild = _provider.GetRoutine<EchoRoutine>("firstChild");
		AddChildRoutine(firstChild, re);
		
		return new StageRoutineResult()
		{
			Result = RoutineOperand
		};
	}
}