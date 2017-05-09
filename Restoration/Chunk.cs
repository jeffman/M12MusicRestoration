using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class Chunk
    {
        public byte[] Data { get; set; }
        public int Pointer { get; set; }

        public static Chunk FromRom(byte[] rom, int address)
        {
            int length = rom.ReadUShort(address);

            if (length == 0)
                return null;

            int pointer = rom.ReadUShort(address + 2);
            byte[] data = rom.ReadBytes(address + 4, length);

            return new Chunk { Data = data, Pointer = pointer };
        }

        public override string ToString()
        {
            return $"Length: 0x{Data.Length:X}, Pointer: 0x{Pointer:X}";
        }
    }
}
