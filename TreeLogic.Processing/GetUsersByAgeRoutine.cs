using TestDomain;
using TreeLogic.Core;
using TreeLogic.Core.Abstractions;
using TreeLogic.Core.Data;

namespace TreeLogic.Processing;

public class GetUsersByAgeRoutine: Routine
{
	private readonly IRoutineManager _routineManager;

	public GetUsersByAgeRoutine(IRoutineManager rm)
	{
		_routineManager = rm;
	}
	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		var age = (int)this.RoutineOperand;
		
		Func<IQueryable<USER>, IQueryable<USER>> query = (users) => users.Where(x => x.Age > age);
		var result = _routineManager.Go<QueryDataObjectRoutine<USER>>(query);

		return result.PrepareResult;
	}
}