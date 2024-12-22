namespace TreeLogic.Core.Abstractions;

public interface ITransactionEnvironment
{
	void OpenTransactionEnvironment();
	
	void CloseTransactionEnvironment();
}