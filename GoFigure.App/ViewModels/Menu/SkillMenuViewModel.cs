using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;

namespace GoFigure.App.ViewModels.Menu
{
    class SkillMenuViewModel : HelpMenuViewModel, IHandle<ZeroDataMessages>
    {
        private readonly GameSettings _gameSettings;

        public bool BeginnerSkill => _gameSettings.CurrentSkill == Skill.Beginner;

        public bool IntermediateSkill => _gameSettings.CurrentSkill == Skill.Intermediate;

        public bool ExpertSkill => _gameSettings.CurrentSkill == Skill.Expert;

        public SkillMenuViewModel(
            IEventAggregator eventAggregator,
            GameSettings gameSettings
        ) : base(eventAggregator)
        {
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
            _gameSettings.CurrentSkill = skill;

            await PublishMessage(ZeroDataMessages.GameSettingsChanged);
        }

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken _)
        {
            if (message != ZeroDataMessages.GameSettingsChanged)
            {
                return;
            }

            NotifyOfPropertyChange(() => BeginnerSkill);
            NotifyOfPropertyChange(() => IntermediateSkill);
            NotifyOfPropertyChange(() => ExpertSkill);
        }
    }
}
