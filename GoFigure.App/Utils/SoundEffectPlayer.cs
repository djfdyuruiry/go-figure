using System.Media;
using System.Threading.Tasks;

using GoFigure.App.Model;

namespace GoFigure.App.Utils
{
    public class SoundEffectPlayer : ISoundEffectPlayer
    {
        public bool Enabled { get; set; }

        public void Play(SoundEffectSource effect)
        {        
            using (var soundStream = effect.Load())
            {
                var soundPlayer = new SoundPlayer(soundStream);

                soundPlayer.Load();
                soundPlayer.PlaySync();
            }
        }

        public SoundEffectPlayer() =>
            Enabled = true;
    }
}
