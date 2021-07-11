using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils;

using static GoFigure.App.Constants;

namespace GoFigure.App.ViewModels.Menu
{
    public class SkillMenuViewModel : HelpMenuViewModel,
                                      IHandle<NewGameStartedMessage>,
                                      IHandle<ZeroDataMessage>
    {
        protected readonly IMessageBoxManager _messageBoxManager;
        protected readonly GameSettings _gameSettings;

        private bool _gameInProgess;

        public bool BeginnerSkill => _gameSettings.CurrentSkill == Skill.Beginner;

        public bool IntermediateSkill => _gameSettings.CurrentSkill == Skill.Intermediate;

        public bool ExpertSkill => _gameSettings.CurrentSkill == Skill.Expert;

        public SkillMenuViewModel(
            IEventAggregator eventAggregator,
            IMessageBoxManager messageBoxManager,
            GameSettings gameSettings
        ) : base(eventAggregator)
        {
            _messageBoxManager = messageBoxManager;
            _gameSettings = gameSettings;
        }

        public async void UseBeginnerSkill() =>
            await SetSkill(Skill.Beginner);

        public async void UseIntermediateSkill() =>
            await SetSkill(Skill.Intermediate);

        public async void UseExpertSkill() =>
            await SetSkill(Skill.Expert);

        private async Task SetSkill(Skill skill)
        {
            if (_gameInProgess
                && _messageBoxManager.ShowOkCancel(OperatorPrecedenceChangeMessage) != MessageBoxResult.OK)
            {
                NotifyOfPropertyChange(() => BeginnerSkill);
                NotifyOfPropertyChange(() => IntermediateSkill);
                NotifyOfPropertyChange(() => ExpertSkill);

                return;
            }

            _gameSettings.CurrentSkill = skill;

            await PublishMessage(ZeroDataMessage.GameSettingsChanged);
        }

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _) => 
            _gameInProgess = true;

        public async Task HandleAsync(ZeroDataMessage message, CancellationToken _)
        {
            if (message != ZeroDataMessage.GameSettingsChanged)
            {
                return;
            }

            NotifyOfPropertyChange(() => BeginnerSkill);
            NotifyOfPropertyChange(() => IntermediateSkill);
            NotifyOfPropertyChange(() => ExpertSkill);
        }
    }
}
