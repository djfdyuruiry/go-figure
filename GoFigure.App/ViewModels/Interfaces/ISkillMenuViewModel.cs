using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoFigure.App.ViewModels.Interfaces
{
  public interface ISkillMenuViewModel : IHelpMenuViewModel
  {
    bool BeginnerSkill { get; }

    bool ExpertSkill { get; }

    bool IntermediateSkill { get; }

    Task PublishNewGameMessage();

    void UseBeginnerSkill(MenuItem view);

    void UseExpertSkill(MenuItem view);

    void UseIntermediateSkill(MenuItem view);
  }
}
