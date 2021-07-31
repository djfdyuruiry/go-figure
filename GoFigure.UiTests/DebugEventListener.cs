using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GoFigure.UiTests
{
  public class DebugEventListener
  {
    private readonly StreamReader _standardOut;
    private readonly CancellationTokenSource _cancelTokenSource;

    private CancellationToken _cancelToken;
    private Task _task;

    public List<string> CurrentSolution { get; set; } 

    public DebugEventListener(StreamReader standardOut)
    {
      _standardOut = standardOut;
      _cancelTokenSource = new CancellationTokenSource();

      CurrentSolution = new List<string>();
    }

    private void Run()
    {
      while (!_cancelToken.IsCancellationRequested)
      {
        var readTask = _standardOut.ReadLineAsync();

        readTask.Wait(_cancelToken);

        var line = readTask.Result;

        if (string.IsNullOrWhiteSpace(line))
        {
          continue;
        }

        var kvp = line.Trim().ToLower().Split("=");

        if (kvp.Length < 2)
        {
          continue;
        }

        if (kvp[0].Trim() == "solution")
        {
          CurrentSolution = new List<string>(
            kvp[1].Trim().Split(",")
          );
        }
      }
    }

    public void Start()
    {
      _cancelToken = _cancelTokenSource.Token;

      _task = new Task(Run, _cancelToken);
      _task.Start();
    }

    public void Stop() => 
      _cancelTokenSource?.Cancel();
  }
}
