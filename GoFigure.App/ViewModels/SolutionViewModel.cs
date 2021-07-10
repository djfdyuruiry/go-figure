using System;
using System.Collections.Generic;
using System.Linq;
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
                              IHandle<ZeroDataMessage>
    {
        private readonly SolutionComputer _computer;
        private readonly IDictionary<int, Expression<Func<string>>> _indexToSlotProperty;
        private readonly SolutionGenerator _generator;
        private readonly SolutionPlan _userSolution;
        
        private int _currentLevel;
        private SolutionPlan _cpuSolution;
        private IDictionary<int, int> _cpuSolutionNumbers;
        private int _currentSlotIndex;
        private int _hintsLeft;
        private bool _controlsEnabled;

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

        public SolutionViewModel(IEventAggregator eventAggregator, 
                                 SolutionComputer computer,
                                 SolutionGenerator generator) : base(eventAggregator)
        {
            _computer = computer;
            _generator = generator;
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
            _hintsLeft = MaxHints;

            _userSolution = new SolutionPlan();
        }

        public void SetSlotIndex(int index) =>
            CurrentSlotIndex = index;

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            _currentLevel = message.Level;

            _cpuSolution = message.Solution;
            _cpuSolutionNumbers = _cpuSolution.AvailableNumbers
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count());

            _hintsLeft = MaxHints;

            ClearSolution();

            NotifyOfPropertyChange(() => SolutionResult);
            ControlsEnabled = true;
        }

        public async Task HandleAsync(SetSolutionSlotMessage message, CancellationToken _)
        {
            if (AllValueInstancesAreInUse(message.Value))
            {
                return;
            }

            _userSolution.Slots[CurrentSlotIndex] = message.Value;

            NotifyOfPropertyChange(
                _indexToSlotProperty[CurrentSlotIndex]
            );
            
            if (CurrentSlotIndex != _cpuSolution.Slots.Count - 1)
            {
                CurrentSlotIndex++;
            }
        }

        public async Task HandleAsync(ZeroDataMessage message, CancellationToken __)
        {
            if (message != ZeroDataMessage.SubmitSolution 
                && message != ZeroDataMessage.ShowSolutionHint
                && message != ZeroDataMessage.ClearSolution)
            {
                return;
            }

            if (message == ZeroDataMessage.ClearSolution)
            {
                ClearSolution();
                return;
            }

            if (message == ZeroDataMessage.ShowSolutionHint)
            {
                await ShowSolutionHint();
                return;
            }

            await CheckIfSolutionValid();
        }

        private bool AllValueInstancesAreInUse(ISolutionSlotValue value)
        {
            var numberSlotValue = value as NumberSlotValue;

            if (_cpuSolutionNumbers is null || numberSlotValue is null)
            {
                return false;
            }

            var number = numberSlotValue.Value;
            var userNumberCounts = _userSolution.AvailableNumbers
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count());

            if (!userNumberCounts.ContainsKey(number)
                || userNumberCounts[number] < 1)
            {
                return false;
            }

            var remainingCounts = userNumberCounts.ToDictionary(
                kvp => kvp.Key,
                kvp => _cpuSolutionNumbers[kvp.Key] - kvp.Value
            );

            return !remainingCounts.ContainsKey(number)
                || remainingCounts[number] < 1;
        }

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

        private void ClearSolution()
        {
            _userSolution.Slots.Clear();

            for (int i = 0; i < _cpuSolution.Slots.Count; i++)
            {
                _userSolution.Slots.Add(null);

                NotifyOfPropertyChange(
                    _indexToSlotProperty[i]
                );
            }

            CurrentSlotIndex = 0;
        }

        private async Task ShowSolutionHint()
        {
            if (_hintsLeft == 3)
            {
                ShowHintInSolutionSlot(0);
                ShowHintInSolutionSlot(1);
            }
            else if (_hintsLeft == 2)
            {
                ShowHintInSolutionSlot(2);
                ShowHintInSolutionSlot(3);
            }
            else if (_hintsLeft == 1)
            {
                ShowHintInSolutionSlot(4);
                ShowHintInSolutionSlot(5);

                await PublishMessage(ZeroDataMessage.NoHintsLeft);
            }

            if (_hintsLeft > 0)
            {
                _hintsLeft--;
            }
        }

        private void ShowHintInSolutionSlot(int slotIndex)
        {
            _userSolution.Slots[slotIndex] = _cpuSolution.Slots[slotIndex];

            NotifyOfPropertyChange(
                _indexToSlotProperty[slotIndex]
            );
        }

        private async Task CheckIfSolutionValid()
        {
            var userSolutionIsWellFormed = _userSolution.IsWellFormed;
            var solutionValid = userSolutionIsWellFormed
                && _userSolution.Slots.Count == _userSolution.Slots.Count
                && _computer.ResultFor(_userSolution) == _computer.ResultFor(_cpuSolution);
            var userMessage = solutionValid 
                ? CorrectSolutionMessage
                : IncorrectSolutionMessage;

            if (userSolutionIsWellFormed)
            {
                NotifyOfPropertyChange(() => SolutionResult);
            }

            await PublishMessage(ZeroDataMessage.PauseGame);

            MessageBox.Show(MessageBoxHeader, userMessage, MessageBoxButton.OK);

            if (!solutionValid)
            {
                await PublishMessage(ZeroDataMessage.ResumeGame);

                return;
            }

            await MoveToNextLevel();
        }

        private async Task MoveToNextLevel()
        {
            var nextLevel = _currentLevel + 1;

            await PublishMessage(
                new NewGameStartedMessage
                {
                    Level = nextLevel,
                    Solution = _generator.Generate(nextLevel)
                }
            );
        }
    }
}
