using System;
using System.Threading;
using System.Threading.Tasks;

namespace GoFigure.Tests
{
  public static class TestUtils
  {
    public static async Task RunOnUiThread(Action test)
    {
      var tcs = new TaskCompletionSource<bool>();
      var thread = new Thread(() =>
      {
        try
        {
          test?.Invoke();
          tcs.SetResult(true);
        }
        catch (Exception e)
        {
          tcs.SetException(e);
        }
      });

      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();

      await tcs.Task;
    }
  }
}
