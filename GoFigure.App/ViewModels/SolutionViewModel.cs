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
using GoFigure.App.Model.Settings;
using GoFigure.App.Model.Solution;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Interfaces;
using static GoFigure.App.Constants;

namespace GoFigure.App.ViewModels
{
    public class SolutionViewModel : BaseControlViewModel,
                              ISolutionViewModel,
                              IHandle<NewGameStartedMessage>,
                              IHandle<SetSolutionSlotMessage>,
                              IHandle<SubmitSolutionMessage>,
                              IHandle<ZeroDataMessage>
    {
        private const string DefaultSlotBackground = "White";
        private const string DisabledSlotBackground = "Black";

        private readonly ISolutionComputer _computer;
        private readonly ISolutionGenerator _generator;
        private readonly IMessageBoxManager _messageBoxManager;
        private readonly GameSettings _gameSettings;
        private readonly IDictionary<int, Expression<Func<string>>> _indexToSlotProperty;
        private readonly SolutionPlan _userSolution;

        private int _currentLevel;
        private SolutionPlan _cpuSolution;
        private IDictionary<int, int> _cpuSolutionNumbers;
        private int _currentSlotIndex;
        private int _hintsLeft;
        private bool _controlsEnabled;
        private bool _ignoreControls;
        private string _slotBackground;

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

        public string SlotBackground
        {
            get => _slotBackground;
            set
            {
                _slotBackground = value;

                NotifyOfPropertyChange(() => SlotBackground);
            }
        }

        public SolutionViewModel(
            IEventAggregatorWrapper eventAggregator,
            ISolutionComputer computer,
            ISolutionGenerator generator,
            IMessageBoxManager messageBoxManager,
            GameSettings gameSettings
        ) : base(eventAggregator)
        {
            _computer = computer;
            _generator = generator;
            _messageBoxManager = messageBoxManager;
            _gameSettings = gameSettings;
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
            _userSolution = new SolutionPlan();

            _hintsLeft = _gameSettings.CurrentSkillLevel.MaxHints;

            SlotBackground = DefaultSlotBackground;
        }

        public void SetSlotIndex(int index)
        {
            if (_ignoreControls)
            {
                return;
            }

            CurrentSlotIndex = index;
        }

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            _currentLevel = message.Level;

            _cpuSolution = message.Solution;
            _cpuSolutionNumbers = _cpuSolution.AvailableNumbers
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count());

            _hintsLeft = _gameSettings.CurrentSkillLevel.MaxHints;

            ClearSolution();

            NotifyOfPropertyChange(() => SolutionResult);

            ControlsEnabled = true;
            SlotBackground = DefaultSlotBackground;
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

        public async Task HandleAsync(SubmitSolutionMessage message, CancellationToken _) =>
            await CheckIfSolutionValid(message.ActiveWindow);

        public async Task HandleAsync(ZeroDataMessage message, CancellationToken __)
        {
            if (!message.IsOneOf(
                ZeroDataMessage.ShowSolutionHint,
                ZeroDataMessage.ClearSolution,
                ZeroDataMessage.PauseGame,
                ZeroDataMessage.ResumeGame
            ))
            {
                return;
            }

            if (message is ZeroDataMessage.ClearSolution)
            {
                ClearSolution();
            }
            else if (message is ZeroDataMessage.ShowSolutionHint)
            {
                await ShowSolutionHint();
            }
            else
            {
                SlotBackground = message is ZeroDataMessage.PauseGame
                    ? DisabledSlotBackground
                    : DefaultSlotBackground;
                _ignoreControls = message is ZeroDataMessage.PauseGame;
            }
        }

        private string SlotValueOrDefault(int index) =>
            index switch
            {
                _ when _userSolution.Slots.Count == 0
                    || _userSolution.Slots.Count < index => string.Empty,
                _ when _userSolution.Slots[index] is null => string.Empty,
                _ => _userSolution.Slots[index] is NumberSlotValue
                    ? $"{_userSolution.Slots[index].As<NumberSlotValue>().Value}"
                    : $"{_userSolution.Slots[index].As<OperatorSlotValue>().Value.ToCharacter()}"
            };

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

        private async Task CheckIfSolutionValid(Window activeWindow)
        {
            if (!CheckNumberUseCountCorrect())
            {
                await PublishMessage(ZeroDataMessage.PauseTimer);
                _messageBoxManager.ShowWarning(activeWindow, TooManyNumberUsesMessage);
                await PublishMessage(ZeroDataMessage.ResumeTimer);

                return;
            }

            var userSolutionIsWellFormed = _userSolution.IsWellFormed;
            var solutionValid = userSolutionIsWellFormed
                && _userSolution.Slots.Count == _cpuSolution.Slots.Count
                && _computer.ResultFor(_userSolution) == _computer.ResultFor(_cpuSolution);
            var userMessage = solutionValid
                ? CorrectSolutionMessage
                : IncorrectSolutionMessage;

            if (userSolutionIsWellFormed)
            {
                NotifyOfPropertyChange(() => SolutionResult);
            }

            await PublishMessage(ZeroDataMessage.PauseTimer);

            _messageBoxManager.ShowInformation(activeWindow, userMessage);

            if (!solutionValid)
            {
                await PublishMessage(ZeroDataMessage.ResumeTimer);

                return;
            }

            await MoveToNextLevel();
        }

        private bool CheckNumberUseCountCorrect()
        {
            if (_cpuSolutionNumbers is null)
            {
                return true;
            }

            var numberCounts = _cpuSolution.AvailableNumbers
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count());

            var userNumberCounts = _userSolution.AvailableNumbers
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count());

            var remainingCounts = numberCounts.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value - userNumberCounts.GetValueOrDefault(kvp.Key, 0)
            );

            return remainingCounts.All(kvp => kvp.Value == 0);
        }

        private async Task MoveToNextLevel() =>
            await PublishMessage(
                new NewGameStartedMessage
                {
                    Level = _currentLevel + 1,
                    Solution = _generator.Generate(_currentLevel + 1, _computer.ResultFor(_cpuSolution))
                }
            );
    }
}
