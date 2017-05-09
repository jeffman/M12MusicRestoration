using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public static class Extensions
    {
        public static byte[] ReadBytes(this byte[] rom, int address, int length)
        {
            byte[] bytes = new byte[length];
            Array.Copy(rom, address, bytes, 0, length);
            return bytes;
        }

        public static ushort ReadUShort(this byte[] rom, int address)
        {
            return (ushort)(rom[address] | (rom[address + 1] << 8));
        }

        public static ushort ReadUShortBigEndian(this byte[] rom, int address)
        {
            return (ushort)(rom[address + 1] | (rom[address] << 8));
        }

        public static int ReadUInt24(this byte[] rom, int address)
        {
            return rom[address] | (rom[address + 1] << 8) | (rom[address + 2] << 16);
        }

        public static int ReadSnesPointer(this byte[] rom, int address)
        {
            return rom.ReadUInt24(address) - 0xC00000;
        }

        // High-Low-Mid byte order
        public static int ReadHLMPointer(this byte[] rom, int address)
        {
            return (rom[address + 1] | (rom[address + 2] << 8) | (rom[address] << 16)) - 0xC00000; 
        }

        public static void WriteInt(this byte[] rom, int address, int value)
        {
            rom[address++] = (byte)(value & 0xFF);
            rom[address++] = (byte)((value >> 8) & 0xFF);
            rom[address++] = (byte)((value >> 16) & 0xFF);
            rom[address++] = (byte)((value >> 24) & 0xFF);
        }

        public static void WriteGbaPointer(this byte[] rom, int address, int pointer)
        {
            WriteInt(rom, address, pointer | 0x8000000);
        }

        public static int Align(this int value, int alignment)
        {
            if (alignment < 1)
                throw new InvalidOperationException("Alignment must be positive");

            if (value < 0)
                throw new InvalidOperationException("Value must be non-negative");

            if (alignment == 1)
                return value;

            int mask = -alignment;
            value += alignment - 1;
            value &= mask;
            return value;
        }
    }
}
