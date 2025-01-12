using TreeLogic.Core.Abstractions;

namespace TreeLogic.Core;

public class TransactionProviderManager: ITransactionProviderManager
{
	private Dictionary<string, ITransactionProvider> _transactionProviders;

	public TransactionProviderManager()
	{
		_transactionProviders = new Dictionary<string, ITransactionProvider>();
	}

	public void RegisterTransactionProvider(string transactionType, ITransactionProvider transactionProvider)
	{
		_transactionProviders.Add(transactionType, transactionProvider);
	}

	public ITransactionProvider GetProvider(string transactionType)
	{
		if (_transactionProviders.TryGetValue(transactionType, out var provider))
		{
			return provider;
		}

		return null;
	}
}