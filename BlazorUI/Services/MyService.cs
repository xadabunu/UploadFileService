namespace BlazorUI.Components.Services;

public class MyService : IMyService
{
    private int _number { get; }

    public MyService()
    {
        _number = new Random().Next();
    }

    public int GetNum()
    {
        return _number;
    }
}