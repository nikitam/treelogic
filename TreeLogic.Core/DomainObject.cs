namespace TreeLogic.Core;

public class DomainObject
{
	private Dictionary<Type, Object> _domains;

	public DomainObject()
	{
		_domains = new Dictionary<Type, object>();
	}
}