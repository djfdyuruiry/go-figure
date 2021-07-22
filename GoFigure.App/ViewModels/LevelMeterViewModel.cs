using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.App.ViewModels
{
  public class LevelMeterViewModel : BaseControlViewModel, 
                     ILevelMeterViewModel,
                     IHandle<NewGameStartedMessage>
  {
    private int _level;

    public string Level2Fill => GetFillForLevel(2);

    public string Level3Fill => GetFillForLevel(3);

    public string Level4Fill => GetFillForLevel(4);

    public string Level5Fill => GetFillForLevel(5);

    public string Level6Fill => GetFillForLevel(6);

    public string Level7Fill => GetFillForLevel(7);

    public string Level8Fill => GetFillForLevel(8);

    public string Level9Fill => GetFillForLevel(9);

    public string Level10Fill => GetFillForLevel(10);

    public LevelMeterViewModel(IEventAggregatorWrapper eventAggregator) : base(eventAggregator)
    {
    }

    public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
    {
      _level = message.Level;

      NotifyOfPropertyChange(() => Level2Fill);
      NotifyOfPropertyChange(() => Level3Fill);
      NotifyOfPropertyChange(() => Level4Fill);
      NotifyOfPropertyChange(() => Level5Fill);
      NotifyOfPropertyChange(() => Level6Fill);
      NotifyOfPropertyChange(() => Level7Fill);
      NotifyOfPropertyChange(() => Level8Fill);
      NotifyOfPropertyChange(() => Level9Fill);
      NotifyOfPropertyChange(() => Level10Fill);
    }

    private string GetFillForLevel(int level) =>
      level > _level
        ? "Transparent"
        : "Red";
  }
}
