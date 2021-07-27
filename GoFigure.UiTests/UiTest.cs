using System.Reflection;

using Xunit.Sdk;

namespace GoFigure.UiTests
{
  public class UiTest : BeforeAfterTestAttribute
  {
    public override void Before(MethodInfo methodUnderTest) =>
      UiTestBase.WithCurrentUiTest(t =>
        t.InitUiTestContext(methodUnderTest.DeclaringType.Name, methodUnderTest.Name)
      );
  }
}
