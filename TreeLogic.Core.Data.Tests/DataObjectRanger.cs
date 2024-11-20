namespace TreeLogic.Core.Data.Tests;

public class DataObjectRangerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var ap = new AssemblyProvider();
        var dos = new DataObjectRanger(ap);

        var userLevel = dos.GetDataObjectLevel(typeof(User));
        var roleLevel = dos.GetDataObjectLevel(typeof(Role));
        var tenantLevel = dos.GetDataObjectLevel(typeof(Tenant));

        Assert.IsTrue(tenantLevel == 2);
        Assert.IsTrue(userLevel == 3);
        Assert.IsTrue(roleLevel == 1);
    }
}