using System.Threading.Tasks;

using GoFigure.App.Views;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface IControlsViewModel
  {
    bool ControlsEnabled { get; set; }
    
    bool HintEnabled { get; set; }
    
    string Number1 { get; }
    
    string Number2 { get; }
    
    string Number3 { get; }
    
    string Number4 { get; }

    Task EnterNumberIntoSolution(int numberIndex);
    
    Task EnterOperatorIntoSolution(char operatorSymbol);

    Task ShowSolutionHint();
    
    Task SubmitSolution(ControlsView view);
  }
}
