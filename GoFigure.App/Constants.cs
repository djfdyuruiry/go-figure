using System;
using System.Collections.Generic;
using System.IO;

using GoFigure.App.Model;
using GoFigure.App.Properties;

namespace GoFigure.App
{
  public static class Constants
  {
    // generator
    public const int OperatorsPerSolution = 3; // solution length is (this * 2 + 1)

    // settings
    public const string SettingsFilename = "game-settings.yml";
    public static readonly string SettingsPath = Path.Join(AppContext.BaseDirectory, SettingsFilename);

    // user messages
    public const string MessageBoxHeader = "Go Figure!";
    public const string OperatorPrecedenceChangeMessage = "Changing operator precedence will start a new game. Continue?";
    public const string SkillChangeMessage = "Changing skill will start a new game. Continue?";
    public const string TooManyNumberUsesMessage = "Each number must be used once! Please try again";
    public const string IncorrectSolutionMessage = "Solution Incorrect! Please try again";
    public const string CorrectSolutionMessage = "Solution correct! Moving to next level";

    public static readonly IDictionary<SoundEffect, SoundEffectSource> SoundEffects =
      new Dictionary<SoundEffect, SoundEffectSource>
      {
        {
          SoundEffect.Alert,
          new SoundEffectSource(
            SoundEffect.Alert,
            () => Resources.alert_sound
          )
        }
      };
  }
}
