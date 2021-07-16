using System.Collections.Generic;
using GoFigure.App.Model;
using GoFigure.App.Model.Settings;
using GoFigure.App.Properties;

namespace GoFigure.App
{
    public static class Constants
    {
        // generator
        public const int OperatorsPerSolution = 3; // solution length is (this * 2 + 1)

        // skill levels
        public static readonly IDictionary<Skill, SkillRules> SkillLevels =
            new Dictionary<Skill, SkillRules>
            {
                { 
                    Skill.Beginner,
                    new SkillRules
                    {
                        MaxTimeInSeconds = 600,
                        
                        MinRandom = 2,
                        MaxRandom = 9,

                        LevelMaxRandomModifers = new List<int>
                        {
                            0, 1, 1, 2, 2, 2, 4, 4, 5, 5
                        },

                        MinTarget = 4,
                        MaxTarget = 250,

                        LevelMinTargetModifers = new List<int>
                        {
                            0, 1, 1, 1, 2, 2, 3, 3, 3, 3
                        },
                        LevelMaxTargetModifers = new List<int>
                        {
                            0, 25, 25, 50, 75, 100, 200, 300, 400, 450
                        },

                        MaxHints = 3
                    }
                },
                {
                    Skill.Intermediate,
                    new SkillRules
                    {
                        MaxTimeInSeconds = 300,

                        MinRandom = 5,
                        MaxRandom = 14,

                        LevelMaxRandomModifers = new List<int>
                        {
                            0, 1, 1, 2, 2, 2, 4, 4, 5, 5
                        },

                        MinTarget = 8,
                        MaxTarget = 750,

                        LevelMinTargetModifers = new List<int>
                        {
                            0, 1, 1, 2, 2, 3, 3, 4, 4, 4
                        },
                        LevelMaxTargetModifers = new List<int>
                        {
                            0, 175, 350, 525, 700, 875, 1050, 1225, 1400, 1500
                        },

                        MaxHints = 2
                    }
                },
                {
                    Skill.Expert,
                    new SkillRules
                    {
                        MaxTimeInSeconds = 150,

                        MinRandom = 8,
                        MaxRandom = 19,

                        LevelMaxRandomModifers = new List<int>
                        {
                            0, 2, 4, 6, 8, 16, 17, 18, 19, 20
                        },

                        MinTarget = 12,
                        MaxTarget = 2500,

                        LevelMinTargetModifers = new List<int>
                        {
                            0, 2, 4, 8, 12, 16, 20, 30, 40, 60
                        },
                        LevelMaxTargetModifers = new List<int>
                        {
                            0, 100, 150, 200, 250, 300, 400, 600, 750, 1000
                        },

                        MaxHints = 1
                    }
                }
            };

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
