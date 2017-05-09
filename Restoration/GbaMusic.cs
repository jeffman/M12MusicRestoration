using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class GbaMusic
    {
        public GbaSong Song { get; set; }
        public List<GbaSampleInstrument> Instruments { get; private set; }

        public static GbaMusic FromSnesMusic(SnesMusic music)
        {
            var song = GbaSong.FromSnesSong(music.Song, 0);

            var instrumentSamplePairs = music.Instruments.Select(i => 
                new { Instrument = i, Sample = music.Samples[i.SampleIndex] });

            var instruments = instrumentSamplePairs.Select(i => GbaSampleInstrument.FromSnesInstrument(i.Instrument, i.Sample));

            return new GbaMusic { Song = song, Instruments = instruments.ToList() };
        }

        public void Serialize(byte[] rom, int address)
        {
            // Song header comes first
            var songHeaderCallback = WriteSongHeader(rom, address);
            address += 8 + (Song.Tracks.Count * 4);

            // Instruments next
            int instrumentAddress = address;
            var instrumentCallbacks = new List<Action<int>>();
            for (int i = 0; i < Instruments.Count; i++)
            {
                instrumentCallbacks.Add(Instruments[i].Serialize(rom, address));
                address += 12;
            }

            // Samples
            for (int i = 0; i < Instruments.Count; i++)
            {
                int sampleAddress = address.Align(4);
                address = Instruments[i].Sample.Serialize(rom, sampleAddress);
                instrumentCallbacks[i](sampleAddress);
            }

            // Song
            int[] trackPointers = Song.Serialize(rom, address);

            songHeaderCallback(instrumentAddress, trackPointers);
        }

        private Action<int, int[]> WriteSongHeader(byte[] rom, int address)
        {
            rom[address++] = (byte)Song.Tracks.Count;
            rom[address++] = 0; // unknown
            rom[address++] = 0; // priority
            rom[address++] = 0x9E; // echo feedback

            return (instrumentPointer, trackPointers) =>
            {
                rom.WriteGbaPointer(address, instrumentPointer);
                address += 4;

                foreach (int trackPointer in trackPointers)
                {
                    rom.WriteGbaPointer(address, trackPointer);
                    address += 4;
                }
            };
        }

        private void WriteInstruments(byte[] rom, ref int address)
        {

        }
    }
}
