namespace TreeLogic.Core.Abstractions;

public interface ITransactionEnvironment: IDisposable
{
	void OpenTransactionEnvironment();
	
	void CloseTransactionEnvironment();
}