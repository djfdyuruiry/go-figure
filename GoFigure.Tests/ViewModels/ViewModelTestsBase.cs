using System;
using System.Threading.Tasks;
using System.Windows;

using FakeItEasy;
using FakeItEasy.Configuration;

using GoFigure.App.ViewModels.Interfaces;

namespace GoFigure.Tests.ViewModels
{
  public class ViewModelTestsBase<T>
  {
    protected IEventAggregatorWrapper _eventAggregator;

    protected T _viewModel;

    protected DependencyObject _testUiComponent;

    protected ViewModelTestsBase()
    {
      _eventAggregator = A.Fake<IEventAggregatorWrapper>();
      _testUiComponent = new DependencyObject();
    }

    protected void RunEventSubscribeTest() =>
      A.CallTo(() => 
        _eventAggregator.SubscribeOnPublishedThread(
          A<object>.That.IsEqualTo(_viewModel)
        )
      ).MustHaveHappened();

    protected void AssertMessageWasPublished<U>() =>
      PublishMessageCallMatching<U>(_ => true).MustHaveHappened();

    protected void AssertMessageWasPublished<U>(U message) =>
      PublishMessageCallEqualTo(message).MustHaveHappened();

    protected void AssertMessageWasNotPublished<U>() =>
      PublishMessageCallMatching<U>(_ => true).MustNotHaveHappened();

    protected void AssertMessageWasNotPublished<U>(U message) =>
      PublishMessageCallEqualTo(message).MustNotHaveHappened();

    protected IReturnValueArgumentValidationConfiguration<Task> PublishMessageCallEqualTo<U>(U message) =>
      A.CallTo(() => _eventAggregator.PublishOnCurrentThreadAsync(A<U>.That.IsEqualTo(message)));

    protected IReturnValueArgumentValidationConfiguration<Task> PublishMessageCallMatching<U>(Func<U, bool> predicate) =>
      A.CallTo(() =>
        _eventAggregator.PublishOnCurrentThreadAsync(
          A<U>.That.Matches(u => predicate(u))
        )
      );
  }
}
