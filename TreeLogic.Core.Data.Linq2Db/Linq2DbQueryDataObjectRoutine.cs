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

using LinqToDB;

namespace TreeLogic.Core.Data.Linq2Db;

public  class Linq2DbQueryDataObjectRoutine<T>: QueryDataObjectRoutine<T> where T : DataObject
{
	private readonly Linq2DbDataConnectionProvider _connectionProvider;
	
	public Linq2DbQueryDataObjectRoutine(Linq2DbDataConnectionProvider cp, DataObjectHierarchyWalker hw) : base(hw)
	{
		_connectionProvider = cp;
	}


	protected override HashSet<T> InternalGetDataObjects()
	{
		HashSet<T> result = null;
			
		var query = this.RoutineOperand as Func<IQueryable<T>, IQueryable<T>>;
		if (query == null)
		{
			throw new Exception("Query is not Func<IQueryable<T>, IQueryable<T>>");
		}
			
		using (var connection = _connectionProvider.GetConnection())
		{
			IQueryable<T> queryable = connection.GetTable<T>();
			var fullQuery = query(queryable);
			result = fullQuery.ToHashSet();
		}

		return result;
	}
}