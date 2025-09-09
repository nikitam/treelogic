namespace TreeLogic.Core.Data;

public abstract class QueryDataObjectRoutine<T>: Routine where T: DataObject
{
	private readonly DataObjectHierarchyWalker _hierarchyWalker;

	protected QueryDataObjectRoutine(DataObjectHierarchyWalker hw)
	{
		_hierarchyWalker = hw;
	}
	
	protected abstract HashSet<T> InternalGetDataObjects();

	public sealed override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		var dataObjects = InternalGetDataObjects();

		if (dataObjects != null && dataObjects.Count > 0)
		{
			foreach (var dataObject in dataObjects)
			{
				_hierarchyWalker.Walk(dataObject, x => x.IsNew = false);
			}
		}
		
		return new StageRoutineResult
		{
			Result = dataObjects
		};
	}
}