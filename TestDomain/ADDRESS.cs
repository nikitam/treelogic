using TreeLogic.Core.Data;

namespace TestDomain;

public class ADDRESS: DataObject
{
    public long Id { get; set; }
    
    public string City { get; set; }
    
    public USER User { get; set; }
    
    public long UserId { get; set; }
    
    public bool IsNew
    {
        get { return Id > 0; }
    }

    public override List<DataObject> GetParents()
    {
        return new List<DataObject> { User };
    }

    public override List<IEnumerable<DataObject>> GetChildren()
    {
        return null;
    }
}