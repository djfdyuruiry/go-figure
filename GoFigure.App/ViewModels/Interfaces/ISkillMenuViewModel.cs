using System.Threading.Tasks;
using System.Windows;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface ISkillMenuViewModel : IHelpMenuViewModel
  {
    bool BeginnerSkill { get; }

    bool ExpertSkill { get; }

    bool IntermediateSkill { get; }

    Task PublishNewGameMessage();

    Task UseBeginnerSkill(DependencyObject view);

    Task UseIntermediateSkill(DependencyObject view);

    Task UseExpertSkill(DependencyObject view);
  }
}
