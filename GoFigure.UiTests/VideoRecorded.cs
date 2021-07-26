using System.Reflection;

using Xunit.Sdk;

namespace GoFigure.UiTests
{
  public class VideoRecorded : BeforeAfterTestAttribute
  {
    public override void Before(MethodInfo methodUnderTest) =>
      UiTestBase.WithCurrentUiTest(t =>
        t.InitUiTestContext(methodUnderTest.Name, methodUnderTest.DeclaringType.Name)
      );
  }
}
