using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;

using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils;

using static GoFigure.App.Constants;

namespace GoFigure.App.ViewModels
{
    class SolutionViewModel : BaseViewModel,
                              IHandle<NewGameStartedMessage>,
                              IHandle<SetSolutionSlotMessage>,
                              IHandle<ZeroDataMessages>
    {
        private readonly SolutionComputer _computer;
        private readonly IDictionary<int, Expression<Func<string>>> _indexToSlotProperty;
        private readonly SolutionPlan _userSolution;
        private readonly List<int> _hintIndicesGiven;

        private SolutionPlan _cpuSolution;
        private int _currentSlotIndex;
        private bool _controlsEnabled;

        private string SlotValueOrDefault(int index)
        {
            if (_userSolution.Slots.Count == 0 
                || _userSolution.Slots.Count < index)
            {
                return string.Empty;
            }

            var solutionSlotValue = _userSolution.Slots[index];

            if (solutionSlotValue is null)
            {
                return string.Empty;
            }

            return solutionSlotValue is NumberSlotValue
                ? $"{solutionSlotValue.As<NumberSlotValue>().Value}"
                : $"{solutionSlotValue.As<OperatorSlotValue>().Value.ToCharacter()}";
        }

        public string Slot1 => SlotValueOrDefault(0);

        public string Slot2 => SlotValueOrDefault(1);

        public string Slot3 => SlotValueOrDefault(2);

        public string Slot4 => SlotValueOrDefault(3);

        public string Slot5 => SlotValueOrDefault(4);

        public string Slot6 => SlotValueOrDefault(5);

        public string Slot7 => SlotValueOrDefault(6);

        public int CurrentSlotIndex
        {
            get => _currentSlotIndex;
            private set
            {
                _currentSlotIndex = value;

                NotifyOfPropertyChange(() => CurrentSlotIndex);
            }
        }

        public int SolutionResult => _computer.ResultFor(_userSolution);

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set
            {
                _controlsEnabled = value;

                NotifyOfPropertyChange(() => ControlsEnabled);
            }
        }

        public SolutionViewModel(IEventAggregator eventAggregator, SolutionComputer computer) : base(eventAggregator)
        {
            _computer = computer;
            _indexToSlotProperty = new Dictionary<int, Expression<Func<string>>>
            {
                { 0, () => Slot1 },
                { 1, () => Slot2 },
                { 2, () => Slot3 },
                { 3, () => Slot4 },
                { 4, () => Slot5 },
                { 5, () => Slot6 },
                { 6, () => Slot7 }
            };
            _hintIndicesGiven = new List<int>();

            _userSolution = new SolutionPlan();
        }

        public void SetSlotIndex(int index) =>
            CurrentSlotIndex = index;

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            _cpuSolution = message.Solution;

            _userSolution.Slots.Clear();
            _hintIndicesGiven.Clear();

            for (int i = 0; i < _cpuSolution.Slots.Count; i++)
            {
                _userSolution.Slots.Add(null);

                NotifyOfPropertyChange(
                    _indexToSlotProperty[i]
                );
            }

            CurrentSlotIndex = 0;
            NotifyOfPropertyChange(() => SolutionResult);
            ControlsEnabled = true;
        }

        public async Task HandleAsync(SetSolutionSlotMessage message, CancellationToken _)
        {
            _userSolution.Slots[CurrentSlotIndex] = message.Value;

            NotifyOfPropertyChange(
                _indexToSlotProperty[CurrentSlotIndex]
            );

            if (CurrentSlotIndex != _cpuSolution.Slots.Count - 1)
            {
                CurrentSlotIndex++;
            }
        }

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken __)
        {
            if (message != ZeroDataMessages.SubmitSolution 
                && message != ZeroDataMessages.ShowSolutionHint)
            {
                return;
            }

            if (message == ZeroDataMessages.ShowSolutionHint)
            {
                await ShowSolutionHint();
                return;
            }

            NotifyOfPropertyChange(() => SolutionResult);

            var userMessage = IncorrectSolutionMessage;

            if (_computer.ResultFor(_userSolution) == _computer.ResultFor(_cpuSolution))
            {
                userMessage = CorrectSolutionMessage;
            }

            MessageBox.Show(userMessage);
        }

        private async Task ShowSolutionHint()
        {
            if (_hintIndicesGiven.Count == _cpuSolution.Slots.Count)
            {
                await PublishMessage(ZeroDataMessages.NoHintsLeft);
                return;
            }

            for (int i = 0; i < _cpuSolution.Slots.Count; i++)
            {
                if (_hintIndicesGiven.Contains(i))
                {
                    continue;
                }

                _userSolution.Slots[i] = _cpuSolution.Slots[i];
                _hintIndicesGiven.Add(i);

                NotifyOfPropertyChange(
                    _indexToSlotProperty[i]
                );
                CurrentSlotIndex = i;

                return;
            }
        }
    }
}
