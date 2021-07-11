using System.Collections.Generic;

namespace GoFigure.App.Model.Settings
{
    public class SkillRules
    {
        public int MaxTimeInSeconds { get; set; }

        public int MinRandom { get; set; }
        public int MaxRandom { get; set; }

        public IList<int> LevelMaxRandomModifers { get; set; }

        public int MinTarget { get; set; }
        public int MaxTarget { get; set; }

        public IList<int> LevelMinTargetModifers { get; set; }
        public IList<int> LevelMaxTargetModifers { get; set; }

        public int MaxHints { get; set; }

        public int MaxRandomForLevel(int level) =>
            MaxRandom + LevelMaxRandomModifers[level];

        public int MinTargetForLevel(int level) =>
            MinTarget + LevelMinTargetModifers[level];

        public int MaxTargetForLevel(int level) =>
            MaxTarget + LevelMaxTargetModifers[level];
    }
}
