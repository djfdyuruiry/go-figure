using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace GoFigure.UiTests
{
  public abstract class UiTestBase : IDisposable
  {
    private static readonly string TestRuntimePath = 
      Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
    private static readonly string RecordingsOutputPath =
      Path.Join(TestRuntimePath, "recordings");

    private static UiTestBase CurrentInstance;

    private string _testClassName;
    private string _testMethodName;

    private AutomationBase _automation;
    protected Application _application;

    private VideoRecorder _recorder;
    private string _testMediaPath;
    private string _recordingPath;

    public static void WithCurrentUiTest(Action<UiTestBase> action)
    {
      if (CurrentInstance is null)
      {
        return;
      }

      action?.Invoke(CurrentInstance);
    }

    public UiTestBase()
    {
      CurrentInstance = this;

      _testMediaPath = Path.GetTempPath();
    }

    public void InitUiTestContext(string className, string methodName)
    {
      _testClassName = className;
      _testMethodName = methodName;

      _automation = new UIA3Automation();
      _application = Application.Launch(@"C:\Users\Matthew\src\c#\go-figure\GoFigure.App\bin\Debug\net5.0-windows\win-x64\Go Figure!.exe");

      StartVideoRecorder(SanitizeFileName(_testClassName)).Wait();
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

    protected Window GetMainWindow() =>
      _application.GetMainWindow(_automation);

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
      Capture.MainScreen();

    private async Task InitRecorderSettings(VideoRecorderSettings videoRecorderSettings)
    {
      var ffmpegPath = await DownloadFFMpeg(Path.GetTempPath());

      videoRecorderSettings.ffmpegPath = ffmpegPath;
    }

    private async Task<string> DownloadFFMpeg(string targetFolder)
    {
      var destPath = Path.Combine(targetFolder, "ffmpeg.exe");

      if (File.Exists(destPath))
      {
        return destPath;
      }

      var uri = new Uri($"https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip");
      var archivePath = Path.Combine(Path.GetTempPath(), "ffmpeg.zip");

      using var webClient = new WebClient();

      await webClient.DownloadFileTaskAsync(uri, archivePath);
        
      Directory.CreateDirectory(targetFolder);
        
      await Task.Run(() =>
      {
        using var archive = ZipFile.OpenRead(archivePath);

        var exeEntry = archive.Entries.First(x => x.Name == "ffmpeg.exe");
        exeEntry.ExtractToFile(destPath, true);
      });

      File.Delete(archivePath);

      return destPath;
    }

    private async Task StartVideoRecorder(string videoName)
    {
      SystemInfo.RefreshAll();

      _recordingPath = Path.Combine(_testMediaPath, $"{SanitizeFileName(videoName)}.mkv");

      var videoRecorderSettings = new VideoRecorderSettings
      {
        VideoFormat = VideoFormat.x264,
        VideoQuality = 6,
        TargetVideoPath = _recordingPath
      };
      
      await InitRecorderSettings(videoRecorderSettings);
      
      _recorder = new VideoRecorder(videoRecorderSettings, r =>
      {
        var testName = $"{_testClassName}.{_testMethodName}";
        var img = CaptureImage();
   
        img.ApplyOverlays(
          new InfoOverlay(img)
          {
            RecordTimeSpan = r.RecordTimeSpan,
            OverlayStringFormat =
              @"{rt:hh\:mm\:ss\.fff} / {name} / CPU: {cpu} / RAM: {mem.p.used}/{mem.p.tot} ({mem.p.used.perc}) / " 
                + testName
          },
          new MouseOverlay(img)
        );

        return img;
      });

      await Task.Delay(500);
    }

    private void StopVideoRecorder()
    {
      _recorder?.Stop();
      _recorder?.Dispose();

      if (File.Exists(_recordingPath))
      {
        if (!Directory.Exists(RecordingsOutputPath))
        {
          Directory.CreateDirectory(RecordingsOutputPath);
        }

        var outputPath = Path.Join(RecordingsOutputPath, Path.GetFileName(_recordingPath));

        File.Move(_recordingPath, outputPath, overwrite: true);
      }

      _recorder = null;
    }

    private void TakeScreenShot(string testName)
    {
      var imageName = $"{SanitizeFileName(testName)}.png".Replace(@"\", string.Empty);
      var imagePath = Path.Combine(_testMediaPath, imageName);

      try
      {
        Directory.CreateDirectory(_testMediaPath);
        CaptureImage().ToFile(imagePath);
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(
          $"Failed to save screen shot to directory: {_testMediaPath}, filename: {imageName}, Ex: {ex.Message}"
        );
      }
    }

    private string SanitizeFileName(string fileName) =>
      string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
  }
}
