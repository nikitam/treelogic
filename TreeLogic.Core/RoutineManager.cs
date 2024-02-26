namespace TreeLogic.Core;

public class RoutineManager
{
	public RoutineEnvironment Go(Routine routine)
	{
		var routineEnvironment = new RoutineEnvironment(routine);

		return routineEnvironment;
	}
}