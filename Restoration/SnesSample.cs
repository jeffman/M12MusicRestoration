using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SnesSample
    {
        public short[] Data { get; set; }
        public int LoopPoint { get; set; }

        private static readonly int[,] FilterNumerators =
        {
            { 0, 15, 61, 115 },
            { 0, 0, 15, 13 }
        };

        private static readonly int[,] FilterDenominators =
        {
            { 1, 16, 32, 64 },
            { 1, 1, 16, 16 }
        };

        public static SnesSample FromBrrStream(byte[] rom, int address, int brrLoopPoint)
        {
            var values = DecodeBrrSequence(rom, address);
            int loopPoint = ((brrLoopPoint - address) / 9) * 16;

            if (loopPoint == values.Count)
                loopPoint = -1;

            return new SnesSample { Data = values.ToArray(), LoopPoint = loopPoint };
        }

        private static List<short> DecodeBrrSequence(byte[] rom, int address)
        {
            bool final = false;
            int prevValue = 0;
            int prevPrevValue = 0;
            var values = new List<short>();

            while (!final)
            {
                byte header = rom[address++];
                int range = (header >> 4) & 0xF;
                int filter = (header >> 2) & 0x3;
                int control = header & 0x3;

                final = (control & 1) == 1;

                foreach (int nybble in EnumerateNybbles(rom, address, 8))
                {
                    int value = (range <= 0xC) ? (nybble << (range - 1)) : (nybble & ~0x7FF);

                    switch (filter)
                    {
                        case 0: // Direct
                            break;

                        case 1: // 15/16
                            value += prevValue + ((-prevValue) >> 4);
                            break;

                        case 2: // 61/32 - 15/16
                            value += (prevValue << 1) + ((-((prevValue << 1) + prevValue)) >> 5) - prevPrevValue + (prevPrevValue >> 4);
                            break;

                        case 3: // 115/64 - 13/16
                            value += (prevValue << 1) + ((-(prevValue + (prevValue << 2) + (prevValue << 3))) >> 6) - prevPrevValue + (((prevPrevValue << 1) + prevPrevValue) >> 4);
                            break;
                    }

                    value = Clip15(Clamp16(value));
                    values.Add((short)(value << 1));
                    prevPrevValue = prevValue;
                    prevValue = value;
                }

                address += 8;
            }

            return values;
        }

        private static int Clip15(int value)
        {
            return ((value & 16384) != 0 ? (value | ~16383) : (value & 16383));
        }

        private static int Clamp16(int value)
        {
            return ((value > 32767) ? 32767 : (value < -32768) ? -32768 : value);
        }

        private static IEnumerable<int> EnumerateNybbles(byte[] rom, int address, int byteLength)
        {
            for (int i = 0; i < byteLength; i++)
            {
                sbyte value = (sbyte)rom[address++];
                yield return value >> 4;
                yield return (sbyte)(value << 4) >> 4;
            }
        }

        private static int ApplyFilter(int value, int filterType, int index)
        {
            return value * FilterNumerators[index, filterType] / FilterDenominators[index, filterType];
        }

        public byte[] Flatten(int loops)
        {
            if (loops < 1)
                throw new Exception("Must have at least one loop");

            int expectedSize = (Data.Length + (Data.Length - LoopPoint) * (loops - 1)) * 2;
            var output = new List<byte>(expectedSize);

            for (int i = 0; i < Data.Length; i++)
                FlattenValue(Data[i], output);

            for (int i = 1; i < loops; i++)
                for (int j = LoopPoint; j < Data.Length; j++)
                    FlattenValue(Data[j], output);

            return output.ToArray();
        }

        private static void FlattenValue(short value, List<byte> output)
        {
            output.Add((byte)(value & 0xFF));
            output.Add((byte)((value >> 8) & 0xFF));
        }
    }
}
