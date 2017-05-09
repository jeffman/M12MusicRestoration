using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SnesMusic
    {
        public SnesSong Song { get; set; }
        public List<SnesInstrument> Instruments { get; private set; }
        public List<SnesSample> Samples { get; private set; }

        public static SnesMusic FromEbData(EbMusicData ebData, int songIndex)
        {
            var spc = SpcData.FromEbMusicData(ebData, songIndex);
            var song = SnesSong.FromRom(spc.Data, ebData.SongSpcPointers[songIndex]);
            var instruments = ReadInstruments(spc);
            var samples = ReadSamples(spc, instruments.Max(i => i.SampleIndex) + 1);

            return new SnesMusic { Song = song, Instruments = instruments, Samples = samples };
        }

        private static List<SnesInstrument> ReadInstruments(SpcData spc)
        {
            var instruments = new List<SnesInstrument>();
            for (int i = 0; i < 32; i++)
            {
                instruments.Add(spc.ReadInstrument(i));
            }
            return instruments;
        }

        private static List<SnesSample> ReadSamples(SpcData spc, int count)
        {
            var samples = new List<SnesSample>(count);
            for (int i = 0; i < count; i++)
                samples.Add(spc.DecodeSample(i));
            return samples;
        }
    }
}
