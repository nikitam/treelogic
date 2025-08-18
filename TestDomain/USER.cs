using TreeLogic.Core.Data;

namespace TestDomain;

public class USER: DataObject
{
	public long Id { get; set; }
	
	public string FirstName { get; set; }
	
	public string LastName { get; set; }
	
	public int Age { get; set; }

	public bool IsNew
	{
		get { return Id > 0; }
	}
}