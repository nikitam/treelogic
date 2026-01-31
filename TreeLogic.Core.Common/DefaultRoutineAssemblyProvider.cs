using System.Reflection;
using TreeLogic.Core.Common.Abstractions;

namespace TreeLogic.Core.Common;

public class DefaultRoutineAssemblyProvider: IRoutineAssemblyProvider
{
    private readonly Assembly _routineAssembly;
    
    public DefaultRoutineAssemblyProvider(Assembly routineAssembly)
    {
        _routineAssembly = routineAssembly;
    }
    
    public Assembly GetRoutineAssembly()
    {
        return _routineAssembly;
    }
}