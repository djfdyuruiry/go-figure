using System;
using System.IO;

namespace GoFigure.App.Model
{
    public class SoundEffectSource
    {
        public SoundEffect Key { get; }

        public Func<Stream> Load { get; }

        public SoundEffectSource(SoundEffect key, Func<Stream> loader)
        {
            Key = key;
            Load = loader;
        }
    }
}
