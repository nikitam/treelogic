/*
   Copyright 2025 Nikita Mulyukin <nmulyukin@gmail.com>

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

namespace TreeLogic.Core.Data;


public enum WalkDirection
{
    Children,
    Parents,
    Both
}


public class DataObjectHierarchyWalker
{
    public void Walk(DataObject root, Action<DataObject> action, WalkDirection direction = WalkDirection.Both)
    {
        if (root == null)
        {
            return;
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var visitedObjects = new HashSet<DataObject>();

        WalkRecursive(root, action, visitedObjects, direction);
    }

    private void WalkRecursive(DataObject current, Action<DataObject> action, HashSet<DataObject> visitedObjects, WalkDirection direction)
    {
        if (current == null)
        {
            return;
        }
        
        if (!visitedObjects.Add(current))
        {
            return;
        }

        action(current);

        if (direction == WalkDirection.Children 
            ||
            direction == WalkDirection.Both)
        {
            var childrensCollection = current.GetChildren();
            if (childrensCollection != null)
            {
                foreach (var childrenCollection in childrensCollection)
                {
                    foreach (var child in childrenCollection)
                    {
                        WalkRecursive(child, action, visitedObjects, direction);
                    }
                }
            }
        }

        if (direction == WalkDirection.Parents 
            ||
            direction == WalkDirection.Both)
        {
            var parents = current.GetParents();
            foreach (var parent in parents)
            {
                WalkRecursive(parent, action, visitedObjects, direction);
            }
        }
    }
}
