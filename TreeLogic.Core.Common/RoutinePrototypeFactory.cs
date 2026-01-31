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
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using TreeLogic.Core.Common.Abstractions;

namespace TreeLogic.Core.Common;

public class RoutinePrototypeFactory: IRoutinePrototypeFactory
{
    // private readonly ObjectPool<RoutinePrototype> _routinePool;
    //
    // public RoutinePrototypeFactory(ObjectPool<RoutinePrototype> routinePool)
    // {
    //     _routinePool = routinePool;
    // }
    
    public async Task<RoutinePrototype> GetRoutinePrototypeAsync(Stream routineStream)
    {
        //var result = _routinePool.Get();
        
        using (var streamReader = new StreamReader(routineStream))
        {
            return JsonSerializer.Deserialize<RoutinePrototype>(await streamReader.ReadToEndAsync());
        }
    }
}