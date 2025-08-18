/*
   Copyright 2024 Nikita Mulyukin <nmulyukin@gmail.com>

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

using System;

namespace TreeLogic.Core.Data;

public interface IDataObjectRanger
{
    int GetDataObjectLevel(Type dataObjectType);
    int GetDataObjectLevel<T>(T dataObject) where T: DataObject;
}

public class DataObjectRanger : IDataObjectRanger
{
    private Dictionary<Type,int> _dataObjectLevels;

    public DataObjectRanger(IDomainAssemblyProvider dap)
    {
        _dataObjectLevels = new Dictionary<Type, int>();

        var assembly = dap.GetDomainAssembly();

        var domainTypes = assembly.GetTypes().Where(x => x.IsDataObject()).ToList();

        foreach (var dt in domainTypes)
        {
            _dataObjectLevels[dt] = GetDataObjectLevelInternal(dt);
        }
    }

    private int GetDataObjectLevelInternal(Type dat)
    {
        if (GetDataObjectLevel(dat) > 0)
        {
            return GetDataObjectLevel(dat);
        }
        
        var parents = dat.GetProperties().Where(x => x.PropertyType.IsDataObject()).ToList();
        var level = 0;

        foreach(var pr in parents)
        {
            var testLevel = GetDataObjectLevelInternal(pr.PropertyType);
            if (testLevel > level)
            {
                level = testLevel;
            }
        }

        level += 1;

        return level;
    }
    public int GetDataObjectLevel(Type dataObjectType)
    {
        return _dataObjectLevels.GetValueOrDefault(dataObjectType, -1);
    }

    public int GetDataObjectLevel<T>(T dataObject) where T : DataObject
    {
        return GetDataObjectLevel(typeof(T));
    }
}
