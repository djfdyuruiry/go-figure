using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using GoFigure.UiTests.Screens;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace GoFigure.UiTests
{
  public abstract class UiTestBase : IDisposable
  {
    private const string FfmpegExe = "ffmpeg.exe";
    private const string FfmpegZip = "ffmpeg-release-essentials.zip";
    private const int RecordingStartTimeoutInMs = 10000;

    private static readonly string TestRuntimePath = Directory.GetCurrentDirectory();
    private static readonly string AppExePath = Path.Join(
      TestRuntimePath.Replace("GoFigure.UiTests", "GoFigure.App"), 
      "Go Figure!.exe"
    );

    private static readonly Uri FfmpegUri =
      new Uri($"https://www.gyan.dev/ffmpeg/builds/{FfmpegZip}");
    private static readonly string FfmpegPath = Path.Combine(Path.GetTempPath(), FfmpegExe);

    private static readonly string ScreenshotsOutputPath = Path.Join(TestRuntimePath, "screenshots");
    private static readonly string RecordingsOutputPath = Path.Join(TestRuntimePath, "recordings");

    private static UiTestBase CurrentInstance;

    private string _testClassName;
    private string _testMethodName;

    private AutomationBase _automation;
    private VideoRecorder _recorder;

    protected Application _application;

    protected Window MainWindow => _application.GetMainWindow(_automation);

    protected AppScreen AppScreen => AppScreen.InWindow(MainWindow);

    static UiTestBase()
    {
      if (Directory.Exists(ScreenshotsOutputPath))
      {
        Directory.Delete(ScreenshotsOutputPath, true);
      }

      Directory.CreateDirectory(ScreenshotsOutputPath);

      if (Directory.Exists(RecordingsOutputPath))
      {
        Directory.Delete(RecordingsOutputPath, true);
      }

      Directory.CreateDirectory(RecordingsOutputPath);

      if (!File.Exists(FfmpegPath))
      {
        DownloadFFMpeg();
      }

      SetProcessDPIAware();
    }

    [DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    private static void DownloadFFMpeg()
    {
      var archivePath = Path.Combine(Path.GetTempPath(), FfmpegZip);

      using var webClient = new WebClient();
      webClient.DownloadFile(FfmpegUri, archivePath);

      using var archive = ZipFile.OpenRead(archivePath);
      archive.Entries
        .First(x => x.Name == FfmpegExe)
        .ExtractToFile(FfmpegPath, true);

      File.Delete(archivePath);
    }

    public static void WithCurrentUiTest(Action<UiTestBase> action)
    {
      if (CurrentInstance is null)
      {
        return;
      }

      action?.Invoke(CurrentInstance);
    }

    public UiTestBase() => 
      CurrentInstance = this;

    public void InitUiTestContext(string className, string methodName)
    {
      _testClassName = className;
      _testMethodName = methodName;

      _automation = new UIA3Automation();
      _application = Application.Launch(AppExePath);

      StartVideoRecorder().Wait();
    }

    public void Dispose()
    {
      CurrentInstance = null;

      TakeScreenShot(_testMethodName);

      CloseApplication();

      StopVideoRecorder();

      _automation?.Dispose();
      _automation = null;
    }

    private void CloseApplication()
    {
      _application?.Close();
  
      Retry.WhileFalse(
        () => _application?.HasExited ?? true,
        TimeSpan.FromSeconds(2),
        ignoreException: true
      );
  
      _application?.Dispose();
      _application = null;
    }

    private CaptureImage CaptureImage() => 
      Capture.Rectangle(
        new Rectangle
        {
          X = 0,
          Y = 0,
          Width = MainWindow.BoundingRectangle.Width,
          Height = MainWindow.BoundingRectangle.Height
        }
      );

    private async Task StartVideoRecorder()
    {
      // move window so it gets captured correctly
      MainWindow.Move(0, 0);

      SystemInfo.RefreshAll();

      var outputDirectory = Path.Combine(RecordingsOutputPath, SanitizeFileName(_testClassName));
      var outputPath = Path.Combine(outputDirectory, $"{SanitizeFileName(_testMethodName)}.mkv");

      _recorder = new VideoRecorder(
        new VideoRecorderSettings
        {
          VideoFormat = VideoFormat.x264,
          VideoQuality = 6,
          TargetVideoPath = outputPath,
          ffmpegPath = FfmpegPath
        },
        CaptureFrame
      );

      Directory.CreateDirectory(outputDirectory);
    }

    private CaptureImage CaptureFrame(VideoRecorder recorder)
    {
      var testName = $"{_testClassName}.{_testMethodName}";
      var img = CaptureImage();

      img.ApplyOverlays(
        new InfoOverlay(img)
        {
          RecordTimeSpan = recorder.RecordTimeSpan,
          OverlayStringFormat = testName
            + "\n{rt:hh\\:mm\\:ss\\.fff} | {name} | CPU: {cpu} | RAM: "
            + @"{mem.p.used}/{mem.p.tot} ({mem.p.used.perc})"
        },
        new MouseOverlay(img)
      );

      return img;
    }

    private void StopVideoRecorder()
    {
      _recorder?.Stop();
      _recorder?.Dispose();

      _recorder = null;
    }

    private void TakeScreenShot(string testName)
    {
      var outputDirectory = Path.Combine(ScreenshotsOutputPath, SanitizeFileName(_testClassName));
      var imageFilename = $"{SanitizeFileName(testName)}.png".Replace(@"\", string.Empty);
      var imagePath = Path.Combine(outputDirectory, imageFilename);

      try
      {
        Directory.CreateDirectory(outputDirectory);
        CaptureImage().ToFile(imagePath);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(
          $"Failed to save screen shot to directory: {ScreenshotsOutputPath}" +
            $", filename: {imageFilename}, Ex: {ex.Message}"
        );
      }
    }

    private string SanitizeFileName(string fileName) =>
      string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
  }
}
