using System.IO;
using System.Threading.Tasks;
using System.Text;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using GoFigure.App.Model.Settings;
using GoFigure.App.Properties;

using static GoFigure.App.Constants;

namespace GoFigure.App.Utils
{
    public class GameSettingsStore : IGameSettingsStore
    {
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serializer;

        public GameSettingsStore()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .DisableAliases()
                .Build();
        }

        public async Task<GameSettings> Read()
        {
            if (!File.Exists(SettingsPath))
            {
                var defaultSettings = Encoding.UTF8.GetString(Resources.GameSettings);

                await File.WriteAllTextAsync(SettingsPath, defaultSettings);
            }

            var currentSettings = await File.ReadAllTextAsync(SettingsPath, Encoding.UTF8);

            return _deserializer.Deserialize<GameSettings>(currentSettings);
        }

        public async Task Write(GameSettings currentSettings) =>
            await File.WriteAllTextAsync(
                SettingsPath,
                _serializer.Serialize(currentSettings),
                Encoding.UTF8
            );
    }
}
