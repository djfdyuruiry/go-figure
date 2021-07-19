using System.Collections.Generic;

using YamlDotNet.Serialization;

namespace GoFigure.App.Model.Settings
{
    public class GameSettings
    {
        public bool SoundEnabled { get; set; }

        public bool UseOperatorPrecedence { get; set; }

        public Skill CurrentSkill { get; set; }

        public Dictionary<Skill, SkillRules> SkillLevels { get; set; }

        [YamlIgnore]
        public SkillRules CurrentSkillLevel => SkillLevels[CurrentSkill];
    }
}
