using Caliburn.Micro;
using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Solution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GoFigure.App.ViewModels
{
    class SolutionViewModel : BaseViewModel, IHandle<NewGameStartedMessage>, IHandle<SetSolutionSlotMessage>, IHandle<ZeroDataMessages>
    {
        private readonly IDictionary<int, Expression<Func<string>>> _indexToSlotProperty;
        private readonly ISolutionSlotValue[] _solutionSlots;
        private int _currentSlotIndex;
        private string _solutionResult;
        private bool _controlsEnabled;

        private string SlotValueOrDefault(int index)
        {
            var solutionSlotValue = _solutionSlots[index];

            if (solutionSlotValue is null)
            {
                return string.Empty;
            }

            return solutionSlotValue is NumberSlotValue
                ? $"{(solutionSlotValue as NumberSlotValue).Value}"
                : $"{(solutionSlotValue as OperatorSlotValue).Character}";
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

        public string SolutionResult
        {
            get => _solutionResult;
            private set
            {
                _solutionResult = value;

                NotifyOfPropertyChange(() => SolutionResult);
            }
        }

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set
            {
                _controlsEnabled = value;

                NotifyOfPropertyChange(() => ControlsEnabled);
            }
        }

        public SolutionViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
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

            _solutionSlots = new ISolutionSlotValue[7];
            _solutionResult = "";
        }

        public void SetSlotIndex(int index) =>
            CurrentSlotIndex = index;

        public async Task HandleAsync(NewGameStartedMessage _, CancellationToken __) 
            => ControlsEnabled = true;

        public async Task HandleAsync(SetSolutionSlotMessage message, CancellationToken _)
        {
            _solutionSlots[CurrentSlotIndex] = message.Value;

            NotifyOfPropertyChange(
                _indexToSlotProperty[CurrentSlotIndex]
            );

            CurrentSlotIndex++;
        }

        public async Task HandleAsync(ZeroDataMessages message, CancellationToken __)
        {
            if (message != ZeroDataMessages.SubmitSolution)
            {
                return;
            }

            // TODO: check if solution is valid
        }
    }
}
