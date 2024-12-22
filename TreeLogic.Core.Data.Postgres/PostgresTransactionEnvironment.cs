using Npgsql;
using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core.Data.Postgres;

public class PostgresTransactionEnvironment: ITransactionEnvironment
{
	public void OpenTransactionEnvironment()
	{
		Connection.Open();
	}

	public void CloseTransactionEnvironment()
	{
		Connection.Close();
	}
	
	internal NpgsqlConnection Connection { get; private set; }
}