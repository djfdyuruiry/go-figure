using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using FakeItEasy;
using Xunit;

using GoFigure.App.Model.Messages;
using GoFigure.App.Model.Settings;
using GoFigure.App.Utils.Interfaces;
using GoFigure.App.ViewModels.Menu;

namespace GoFigure.Tests.ViewModels.Menu
{
  public class SkillMenuViewModelTests : ViewModelTestsBase<SkillMenuViewModel>
  {
    private IMessageBoxManager _messageBoxManager;
    private ISolutionGenerator _solutionGenerator;
    private GameSettings _gameSettings;

    public SkillMenuViewModelTests()
    {
      _messageBoxManager = A.Fake<IMessageBoxManager>();
      _solutionGenerator = A.Fake<ISolutionGenerator>();
      _gameSettings = new GameSettings();

      _viewModel = new SkillMenuViewModel(
        _eventAggregator,
        _messageBoxManager,
        _solutionGenerator,
        _gameSettings
      );
    }

    [Fact]
    public void When_ViewModel_Is_Constructed_Then_It_Subscribes_To_Events() =>
      RunEventSubscribeTest();

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Event_Is_Published()
    {
      await _viewModel.PublishNewGameMessage();

      AssertMessageWasPublished<NewGameStartedMessage>();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Solution_Generator_Is_Called()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() => _solutionGenerator.Generate(A<int>._, A<int>._))
        .MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Level_Passed_To_Solution_Generator_Is_Correct()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() =>
        _solutionGenerator.Generate(
          A<int>.That.IsEqualTo(1),
          A<int>._
        )
      ).MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_NumberToIgnore_Passed_To_Solution_Generator_Is_Correct()
    {
      await _viewModel.PublishNewGameMessage();

      A.CallTo(() =>
        _solutionGenerator.Generate(
          A<int>._,
          A<int>.That.IsEqualTo(-1)
        )
      ).MustHaveHappened();
    }

    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Published_Event_Has_Correct_Level()
    {
      await _viewModel.PublishNewGameMessage();

      PublishMessageCallMatching<NewGameStartedMessage>(
        m => m.Level == 1
      ).MustHaveHappened();
    }


    [Fact]
    public async void When_PublishNewGameMessage_Is_Called_Then_Published_Event_Has_Generated_Solution()
    {
      await _viewModel.PublishNewGameMessage();

      PublishMessageCallMatching<NewGameStartedMessage>(
        m => m.Solution != null
      ).MustHaveHappened();
    }

    public static IEnumerable<object[]> GetSkillMethods()
    {
      Func<SkillMenuViewModel, DependencyObject, Task> beginner = (s, d) => s.UseBeginnerSkill(d);
      Func<SkillMenuViewModel, DependencyObject, Task> intermediate = (s, d) => s.UseIntermediateSkill(d);
      Func<SkillMenuViewModel, DependencyObject, Task> expert = (s, d) => s.UseExpertSkill(d);

      yield return new object[] { Skill.Beginner, beginner };
      yield return new object[] { Skill.Intermediate, intermediate };
      yield return new object[] { Skill.Expert, expert };
    }

    [Theory]
    [MemberData(nameof(GetSkillMethods))]
    public async void When_UseSkill_Is_Called_And_Game_Is_Not_InProgess_Then_GameSettings_Event_Is_Published(
      Skill _,
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod
    )
    {
      await useSkillMethod(_viewModel, _testUiComponent);

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Theory]
    [MemberData(nameof(GetSkillMethods))]
    public async void When_UseSkill_Is_Called_And_Game_Is_Not_InProgess_Then_GameSettings_Are_Updated(
      Skill skill,
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod
    )
    {
      await useSkillMethod(_viewModel, _testUiComponent);

      Assert.Equal(skill, _gameSettings.CurrentSkill);
    }

    [Theory]
    [MemberData(nameof(GetSkillMethods))]
    public async void When_UseSkill_Is_Called_And_Game_Is_InProgess_And_User_Clicks_Ok_Then_GameSettings_Event_Is_Published(
      Skill _,
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod
    )
    {
      await RunGameInProgressSkillTest(useSkillMethod, MessageBoxResult.OK);

      AssertMessageWasPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Theory]
    [MemberData(nameof(GetSkillMethods))]
    public async void When_UseSkill_Is_Called_And_Game_Is_InProgess_And_User_Clicks_Ok_Then_GameSettings_Are_Updated(
      Skill skill,
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod
    )
    {
      await RunGameInProgressSkillTest(useSkillMethod, MessageBoxResult.OK);

      Assert.Equal(skill, _gameSettings.CurrentSkill);
    }


    [Theory]
    [MemberData(nameof(GetSkillMethods))]
    public async void When_UseSkill_Is_Called_And_Game_Is_InProgess_And_User_Clicks_Cancel_Then_GameSettings_Event_Is_Not_Published(
      Skill _,
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod
    )
    {
      await RunGameInProgressSkillTest(useSkillMethod, MessageBoxResult.Cancel);

      AssertMessageWasNotPublished(ZeroDataMessage.GameSettingsChanged);
    }

    [Theory]
    [MemberData(nameof(GetSkillMethods))]
    public async void When_UseSkill_Is_Called_And_Game_Is_InProgess_And_User_Clicks_Cancel_Then_GameSettings_Are_Not_Updated(
      Skill _,
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod
    )
    {
      var startingSkill = _gameSettings.CurrentSkill;

      await RunGameInProgressSkillTest(useSkillMethod, MessageBoxResult.Cancel);

      Assert.Equal(startingSkill, _gameSettings.CurrentSkill);
    }

    private async Task RunGameInProgressSkillTest(
      Func<SkillMenuViewModel, DependencyObject, Task> useSkillMethod,
      MessageBoxResult messageBoxResult
    )
    {
      await _viewModel.HandleAsync(new NewGameStartedMessage(), new CancellationToken());

      A.CallTo(() =>
        _messageBoxManager.ShowOkCancel(
          A<DependencyObject>.That.IsEqualTo(_testUiComponent),
          A<string>._)
      ).Returns(messageBoxResult);

      await useSkillMethod(_viewModel, _testUiComponent);
    }
  }
}
