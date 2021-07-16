using System.Threading.Tasks;

using GoFigure.App.Model;

namespace GoFigure.App.Utils
{
    public interface ISoundEffectPlayer
    {
        bool Enabled { get; set; }

        void Play(SoundEffectSource effect);
    }
}
