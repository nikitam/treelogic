using System;
using System.Reflection;

namespace TreeLogic.Core.Data.Tests;

public class User : DataObject
{
    public bool IsNew { get; set; }
    
    public override List<DataObject> GetParents()
    {
        return
        [
            Role,
            Tenant
        ];
    }

    public override List<IEnumerable<DataObject>> GetChildren()
    {
        return null;
    }


    public string FirstName {get;set;}

    public Role Role {get;set;}
    
    public Tenant Tenant {get;set;}
}

public class Role : DataObject
{
    public bool IsNew { get; set; }
    public override List<DataObject> GetParents()
    {
        return null;
    }

    public override List<IEnumerable<DataObject>> GetChildren()
    {
        return
        [
            Tenants
        ];
    }

    public string RoleName {get;set;}
    
    public List<Tenant> Tenants {get;set;}
}

public class Tenant : DataObject
{
    public bool IsNew { get; set; }
    
    public override List<DataObject> GetParents()
    {
        return
        [
            Role
        ];
    }

    public override List<IEnumerable<DataObject>> GetChildren()
    {
        throw new NotImplementedException();
    }

    public string TenantName {get;set;}
    
    public Role Role {get;set;}
}

public class AssemblyProvider : IDomainAssemblyProvider
{
    public Assembly GetDomainAssembly()
    {
        return this.GetType().Assembly;
    }
}
