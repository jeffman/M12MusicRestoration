using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class GbaSample
    {
        public int LoopPoint { get; set; }
        public int Pitch { get; set; }
        public sbyte[] Data { get; set; }

        public static GbaSample FromSnesSample(SnesSample sample, int pitch)
        {
            sbyte[] newData = new sbyte[sample.Data.Length];
            for (int i = 0; i < newData.Length; i++)
            {
                newData[i] = (sbyte)(sample.Data[i] >> 8);
            }
            return new GbaSample { Data = newData, Pitch = pitch, LoopPoint = sample.LoopPoint };
        }

        public int Serialize(byte[] rom, int address)
        {
            rom[address++] = 0; // unused
            rom[address++] = 0; // unused
            rom[address++] = 0; // unused

            if (LoopPoint >= 0)
            {
                rom[address++] = 0x40; // looped
                rom.WriteInt(address + 4, LoopPoint);
            }
            else
            {
                rom[address++] = 0; // unlooped
                rom.WriteInt(address + 4, 0);
            }

            rom.WriteInt(address, Pitch);
            //rom.WriteInt(address, 0x1f40000);

            rom.WriteInt(address + 8, Data.Length);
            address += 12;

            for (int i = 0; i < Data.Length; i++)
            {
                rom[address++] = (byte)Data[i];
            }

            // If looped, write two samples from the loop point to the end
            if (LoopPoint >= 0)
            {
                rom[address++] = (byte)Data[LoopPoint];
                rom[address++] = (byte)Data[LoopPoint + 1];
            }

            return address;
        }
    }
}
