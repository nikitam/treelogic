namespace TreeLogic.Core.Data;

public abstract class QueryDataObjectRoutine<T>: Routine where T: DataObject
{
	protected abstract List<T> InternalGetDataObjects();

	public sealed override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		var result = InternalGetDataObjects();
		
		// recursive walk hierarchy
		// result.ForEach(x => x.IsNew = false);
		
		
		
		return new StageRoutineResult
		{
			Result = result
		};
	}
}