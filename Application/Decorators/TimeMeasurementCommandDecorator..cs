using System.Diagnostics;
using Application.Commands;

namespace Application.Decorators;

public class TimeMeasurementCommandDecorator : ICommand
{
    private readonly ICommand _innerCommand;

    public TimeMeasurementCommandDecorator(ICommand innerCommand)
    {
        _innerCommand = innerCommand;
    }

    public void Execute()
    {
        var sw = Stopwatch.StartNew();
        _innerCommand.Execute();
        sw.Stop();
        Console.WriteLine($"Команда {_innerCommand.GetType().Name} " +
                          $"выполнена за {sw.ElapsedMilliseconds} мс.");
    }
}