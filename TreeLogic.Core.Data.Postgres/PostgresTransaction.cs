using Npgsql;
using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core.Data.Postgres;

public class PostgresTransaction: ITransaction
{
	private NpgsqlTransaction _internalTransaction;
	
	public PostgresTransaction(PostgresTransactionEnvironment pte)
	{
		Environment = pte;
	}
	public void Begin()
	{
		var te = Environment as PostgresTransactionEnvironment;

		_internalTransaction = te.Connection.BeginTransaction();
	}

	public void Commit()
	{
		_internalTransaction.Commit();
	}

	public void Rollback()
	{
		_internalTransaction.Rollback();
	}

	public ITransactionEnvironment Environment { get; }
}