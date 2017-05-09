using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public abstract class GbaInstrument
    {
        public abstract GbaInstrumentType Type { get; }
    }

    public sealed class GbaSampleInstrument : GbaInstrument
    {
        public override GbaInstrumentType Type => GbaInstrumentType.Sample;
        public GbaSample Sample { get; private set; }
        public byte Attack { get; set; }
        public byte Decay { get; set; }
        public byte Sustain { get; set; }
        public byte Release { get; set; }

        public static GbaSampleInstrument FromSnesInstrument(SnesInstrument instrument, SnesSample sample)
        {
            // SNES decay: 0 = longest, 7 = shortest
            // GBA decay: 0 = shortest, FF = longest
            byte decay = (byte)(7 - instrument.Decay);
            decay *= 32;

            // SNES attack: 0 = longest, F = shortest
            // GBA attack: 0 = longest, FF = shortest
            byte attack = (byte)(instrument.Attack * 16);

            // SNES sustain ratio: 0 = lowest, 7 = highest
            // GBA sustain: 0 = lowest, FF = highest
            byte sustain = (byte)(instrument.Sustain * 32);

            // SNES release: 0 = longest, 1F = shortest
            // GBA release: 0 = shortest, FF = longest
            byte release = (byte)(0x1F - instrument.Release);
            release *= 8;

            // Pitch: the SNES instrument pitch adjustment is relative to 0x1000 (4096).
            // That is, a SNES instrument with an adjustment of 0x1000 won't be adjusted at all.
            // I suppose it's all relative to the actual frequency of the sound, so we need to do
            // *something* with the SNES pitch adjustment value.
            // For arbitrary's sake, assume a base of 32768 Hz (which is close to the SPC sampling rate)
            // and do pitch adjustment relative to that.
            int gbaPitch = instrument.Pitch * 0x10000;

            // On the GBA you can only do pitch adjustment on a per-sample basis, as opposed to per-instrument on the SNES.
            // So we'll have to have one sample exclusive to each instrument if we want different pitches,
            // even if the sample data is the same.
            var gbaSample = GbaSample.FromSnesSample(sample, gbaPitch);

            return new GbaSampleInstrument { Sample = gbaSample, Attack = attack, Decay = decay, Sustain = sustain, Release = release };
        }

        public Action<int> Serialize(byte[] rom, int address)
        {
            rom.WriteInt(address, 0x3C00); // type, key, unused, panning
            rom[address + 8] = Attack;
            rom[address + 9] = Decay;
            rom[address + 10] = Sustain;
            rom[address + 11] = Release;

            return samplePointer => rom.WriteGbaPointer(address + 4, samplePointer);
        }
    }

    public enum GbaInstrumentType : byte
    {
        Sample = 0,
        Square1 = 1,
        Square2 = 2,
        Waveform = 3,
        Noise = 4,
        SampleFast = 8,
        KeySplit = 0x40,
        EveryKeySplit = 0x80
    }
}
