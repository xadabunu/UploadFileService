namespace FileSafetyService.EnumerationClasses;

public class ActionMessage
{
    public string Code { get; }
    
    public readonly ActionMessage Delete = new ActionMessage("Delete");
    public readonly ActionMessage Scan = new ActionMessage("Scan");
    
    private ActionMessage(string code)
    {
        Code = code;
    }
}