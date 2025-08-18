using System;
using System.Reflection;

namespace TreeLogic.Core.Data.Tests;

public class User : DataObject
{
    public bool IsNew { get; set; }


    public string FirstName {get;set;}

    public Role Role {get;set;}
    
    public Tenant Tenant {get;set;}
}

public class Role : DataObject
{
    public bool IsNew { get; set; }

    public string RoleName {get;set;}
    
    public List<Tenant> Tenants {get;set;}
}

public class Tenant : DataObject
{
    public bool IsNew { get; set; }
    
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
