using TestDomain;
using TreeLogic.Core;
using TreeLogic.Core.Data.LowLevelProcesses;

namespace TreeLogic.Processing;

public class CreateNewUserRoutine: Routine
{
    private readonly IRoutineManager _routineManager;

    public CreateNewUserRoutine(IRoutineManager rm)
    {
        _routineManager = rm;
    }
    
    
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string CityFirst { get; set; }
    public string CitySecond{ get; set; }
    
    
    public override StageRoutineResult Prepare(RoutineEnvironment re)
    {
        var user = new USER();
        user.FirstName = UserFirstName;
        user.LastName = UserLastName;
        user.Age = 20;
        
        var addr1 = new ADDRESS();
        addr1.City = CityFirst;
        user.Addresses.Add(addr1);
        
        var addr2 = new ADDRESS();
        addr2.City = CitySecond;
        user.Addresses.Add(addr2);

        var result = _routineManager.Go<SaveDataObjectRoutine>(user);
        
        return result.PrepareResult;
    }

    public override void FillByPrototype(RoutinePrototype prototype)
    {
        UserFirstName = prototype.Parameters.Single(x => x.Name == "UserFirstName").Value;
        UserLastName = prototype.Parameters.Single(x => x.Name == "UserLastName").Value;
        CityFirst = prototype.Parameters.Single(x => x.Name == "CityFirst").Value;
        CitySecond = prototype.Parameters.Single(x => x.Name == "CitySecond").Value;
    }
}