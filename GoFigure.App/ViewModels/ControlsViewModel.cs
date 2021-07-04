using Caliburn.Micro;
using GoFigure.App.Model;
using GoFigure.App.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GoFigure.App.ViewModels
{
    class ControlsViewModel : BaseViewModel, IHandle<NewGameStartedMessage>
    {
        private static readonly IDictionary<char, Operator> CharacterToOperator =
            typeof(Operator).GetMembers(BindingFlags.Public | BindingFlags.Static)
                .Select(m => (member: m, charAttributes: m.GetCustomAttributes(typeof(CharacterAttribute), true)))
                .Where(t => t.charAttributes.Length > 0)
                .ToDictionary(
                    t => (t.charAttributes.FirstOrDefault() as CharacterAttribute).Symbol,
                    t => (Operator) Enum.Parse(typeof(Operator), t.member.Name)
                );

        private bool _controlsEnabled;
        private int?[] _numbers;

        private string NumberOrDefault(int index) =>
            _numbers[index].HasValue 
                ? $"{_numbers[index]}"
                : string.Empty;

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set
            {
                _controlsEnabled = value;

                NotifyOfPropertyChange(() => ControlsEnabled);
            }
        }

        public string Number1 => NumberOrDefault(0);

        public string Number2 => NumberOrDefault(1);

        public string Number3 => NumberOrDefault(2);

        public string Number4 => NumberOrDefault(3);

        public ControlsViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            ControlsEnabled = false;
            _numbers = new int?[4];
        }

        public async Task HandleAsync(NewGameStartedMessage message, CancellationToken _)
        {
            message.AvailableNumbers
                .Select((n, idx) => _numbers[idx] = n)
                .ToList();

            NotifyOfPropertyChange(() => Number1);
            NotifyOfPropertyChange(() => Number2);
            NotifyOfPropertyChange(() => Number3);
            NotifyOfPropertyChange(() => Number4);

            ControlsEnabled = true;
        }

        public async void EnterNumberIntoSolution(int numberIndex) => 
            await PublishMessage(
                new NumberEnteredMessage
                {
                    Value = _numbers[numberIndex].Value
                }
            );

        public async void EnterOperatorIntoSolution(char operatorSymbol) =>
            await PublishMessage(
                new OperatorEnteredMessage
                {
                    Value = CharacterToOperator[operatorSymbol]
                }
            );

        public async void SubmitSolution() =>
            await PublishMessage(ZeroDataMessages.SubmitSolution);

        public async void ShowSolutionHint() =>
            await PublishMessage(ZeroDataMessages.ShowSolutionHint);
    }
}
