using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils;

namespace GoFigure.App.ViewModels
{
    class ControlsViewModel : BaseViewModel, IHandle<NewGameStartedMessage>, IHandle<ZeroDataMessages>
    {
        private IList<int> _numbers;
        private bool _controlsEnabled;
        private bool _hintEnabled;

        private string NumberOrDefault(int index) =>
            _numbers.Count == 0 || _numbers.Count - 1 < index
                ? string.Empty
                : $"{_numbers[index]}";

        public string Number1 => NumberOrDefault(0);

        public string Number2 => NumberOrDefault(1);

        public string Number3 => NumberOrDefault(2);

        public string Number4 => NumberOrDefault(3);

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set
            {
                _controlsEnabled = value;

                NotifyOfPropertyChange(() => ControlsEnabled);
            }
        }

        public bool HintEnabled
        {
            get => _hintEnabled;
            set
            {
                _hintEnabled = value;

                NotifyOfPropertyChange(() => HintEnabled);
            }
        }

        public ControlsViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _numbers = new List<int>();

            ControlsEnabled = false;
            HintEnabled = false;
        }

        public async void EnterNumberIntoSolution(int numberIndex) =>
            await PublishMessage(
                new SetSolutionSlotMessage
                {
                    Value = new NumberSlotValue
                    {
                        Value = _numbers[numberIndex]
                    }
                }
            );

        public async void EnterOperatorIntoSolution(char operatorSymbol) =>
            await PublishMessage(
                new SetSolutionSlotMessage
                {
                    Value = new OperatorSlotValue
                    {
                        Value = operatorSymbol.ToOperator()
                    }
                }
            );

        public async void SubmitSolution() =>
            await PublishMessage(ZeroDataMessages.SubmitSolution);

        public async void ShowSolutionHint() =>
            await PublishMessage(ZeroDataMessages.ShowSolutionHint);

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            _numbers = message.Solution
                .AvailableNumbers
                .Shuffle()
                .ToList();

            NotifyOfPropertyChange(() => Number1);
            NotifyOfPropertyChange(() => Number2);
            NotifyOfPropertyChange(() => Number3);
            NotifyOfPropertyChange(() => Number4);

            ControlsEnabled = true;
            HintEnabled = true;
        }

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken _)
        {
            if (message != ZeroDataMessages.NoHintsLeft)
            {
                return;
            }

            HintEnabled = false;
        }
    }
}
