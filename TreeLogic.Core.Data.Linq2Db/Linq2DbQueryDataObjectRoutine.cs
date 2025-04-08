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

public  class Linq2DbQueryDataObjectRoutine<T>: QueryDataObjectRoutine<T> where T : class //where T: class, IDataObject
{
	private readonly Linq2DbDataConnectionProvider _connectionProvider;
	
	public Linq2DbQueryDataObjectRoutine(Linq2DbDataConnectionProvider cp)
	{
		_connectionProvider = cp;
	}
	
	public override StageRoutineResult Prepare(RoutineEnvironment re)
	{
		try
		{
			List<T> result = null;
			
			var query = this.RoutineOperand as Func<IQueryable<T>, IQueryable<T>>;
			if (query == null)
			{
				throw new Exception("Query is not Func<IQueryable<T>, IQueryable<T>>");
			}
			
			//var delegateType = query.GetType();
			//var entityType = GetGenericParameterFromDelegate(delegateType);
			
			using (var connection = _connectionProvider.GetConnection())
			{
				IQueryable<T> queryable = connection.GetTable<T>();
				var fullQuery = query(queryable);
				result = fullQuery.ToList();
			}


			/*Func<IQueryable<DataConnection>, IQueryable<DataConnection>> queryd = x =>
				x.Where(y => y.CommandTimeout == 5).LoadWith(x => x.DataProvider)
					.ThenLoad(x => x.Name);


			var p = new RoutineProvider(null);
			var r = p.GetGenericRoutine<QueryDataObjectRoutine<DataConnection>, DataConnection>(queryd);*/
			
			
			return new StageRoutineResult
			{
				Result = result
			};
		}
		catch (Exception ex)
		{
			return new StageRoutineResult()
			{
				Exception = ex,
			};
		}
	}
}