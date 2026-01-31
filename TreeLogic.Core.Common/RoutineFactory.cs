/*
   Copyright 2026 Nikita Mulyukin <nmulyukin@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Microsoft.Extensions.ObjectPool;
using TreeLogic.Core.Abstractions;
using TreeLogic.Core.Common.Abstractions;

namespace TreeLogic.Core.Common;

public class RoutineFactory: IRoutineFactory
{
    private Dictionary<string, Type> _routineCache;
    private readonly IRoutineAssemblyProvider _routineAssemblyProvider;
    private readonly IRoutineProvider _routineProvider;
    private readonly IRoutinePrototypeFactory _routinePrototypeFactory;
    //private readonly ObjectPool<RoutinePrototype> _routinePool;
    
    public RoutineFactory(IRoutineAssemblyProvider routineAssemblyProvider, IRoutineProvider routineProvider,
        IRoutinePrototypeFactory routinePrototypeFactory
        /*,ObjectPool<RoutinePrototype> routinePool*/)
    {
        _routinePrototypeFactory = routinePrototypeFactory;
        _routineAssemblyProvider = routineAssemblyProvider;
        _routineProvider = routineProvider;
        _routineCache = new Dictionary<string, Type>();
        
        //_routinePool = routinePool;
        
        CacheRoutines();
    }

    private void CacheRoutines()
    {
        var routineAssembly = _routineAssemblyProvider.GetRoutineAssembly();
        foreach (var routineType in routineAssembly.GetTypes().Where(x => x.IsRoutine()))
        {
            _routineCache.Add(routineType.Name, routineType);
        }
    }
    
    public async Task<Routine> GetRoutineAsync(Stream routineStream)
    {
        var prototype = await _routinePrototypeFactory.GetRoutinePrototypeAsync(routineStream);
        
        //try
        //{
            if (_routineCache.TryGetValue(prototype.Name, out var routineType))
            {
                var routine = _routineProvider.GetRoutine(routineType, null);
                routine.FillByPrototype(prototype);
                return routine;
            }

            return null;
        //}
        //finally
        //{
        //    _routinePool.Return(prototype);
        //}
    }
}