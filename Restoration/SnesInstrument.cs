using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SnesInstrument
    {
        public int SampleIndex { get; set; }
        public int Pitch { get; set; }

        public bool UseAdsr { get; set; }
        public int Attack { get; set; }
        public int Decay { get; set; }
        public int Sustain { get; set; }
        public int Release { get; set; }
        public byte Gain { get; set; }

        public static SnesInstrument FromRom(byte[] rom, int address)
        {
            //if (rom.Skip(address).Take(6).All(b => b == 0))
                //return null;

            ushort adsr = rom.ReadUShortBigEndian(address + 1);
            int decay = (adsr >> 12) & 0x7;
            int attack = (adsr >> 8) & 0xF;
            int sustain = (adsr >> 5) & 0x7;
            int release = adsr & 0x1F;

            return new SnesInstrument
            {
                SampleIndex = rom[address],
                UseAdsr = ((adsr & 0x8000) != 0),
                Attack = attack,
                Decay = decay,
                Sustain = sustain,
                Release = release,
                Gain = rom[address + 3],
                Pitch = rom.ReadUShortBigEndian(address + 4)
            };
        }
    }
}
