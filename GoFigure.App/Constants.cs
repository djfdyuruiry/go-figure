using System.Collections.Generic;

using GoFigure.App.Model.Settings;

namespace GoFigure.App
{
    static class Constants
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
                        
                        MinTarget = 4,
                        MaxTarget = 250,

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

                        MinTarget = 8,
                        MaxTarget = 750,

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

                        MinTarget = 12,
                        MaxTarget = 2500,

                        MaxHints = 1
                    }
                }
            };

        // user messages
        public const string MessageBoxHeader = "Go Figure!";
        public const string TooManyNumberUsesMessage = "Each number must be used once! Please try again";
        public const string IncorrectSolutionMessage = "Solution Incorrect! Please try again";
        public const string CorrectSolutionMessage = "Solution correct! Moving to next level";
    }
}
