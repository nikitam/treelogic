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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using TreeLogic.Core.Common.Abstractions;

namespace TreeLogic.Core.Common;

public static class Module
{
    public static IServiceCollection WithCommon(this IServiceCollection serviceCollection, IRoutineAssemblyProvider assemblyProvider)
    {
        serviceCollection.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        serviceCollection.AddSingleton<ObjectPool<RoutinePrototype>>(serviceProvider =>
        {
            var provider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
            var policy = new DefaultPooledObjectPolicy<RoutinePrototype>();
            return provider.Create(policy);
        });
        serviceCollection.AddSingleton<IRoutineAssemblyProvider>(assemblyProvider);
        
        serviceCollection.AddSingleton<IRoutineFactory, RoutineFactory>();
        serviceCollection.AddSingleton<IRoutinePrototypeFactory, RoutinePrototypeFactory>();

        return serviceCollection;
    }
}