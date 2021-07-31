using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;

namespace GoFigure.App.Utils
{
  public class DebugEventListener : IHandle<object>
  {
    private readonly IEventAggregator _eventAggregator;

    public DebugEventListener(IEventAggregator eventAggregator)
    {
      _eventAggregator = eventAggregator;

      _eventAggregator.SubscribeOnPublishedThread(this);
    }

    public async Task HandleAsync(object message, CancellationToken _)
    {
      if (message is NewGameStartedMessage)
      {
        var newGameStartedMessage = message as NewGameStartedMessage;
        var csvSlots = string.Join(
          ",", 
          newGameStartedMessage.Solution
            .Slots
            .Select(s =>
              s is NumberSlotValue 
                ? $"{s.As<NumberSlotValue>().Value}"
                : $"{s.As<OperatorSlotValue>().Value.ToCharacter()}"
            )
        );

        await Console.Out.WriteLineAsync($"solution={csvSlots}");
      }
    }
  }
}
