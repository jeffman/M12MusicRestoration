using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SpcData
    {
        const int SpcSize = 64 * 1024;
        const int SpcSamplesPointer = 0x6C00;
        const int SpcInstrumentsPointer = 0x6E00;

        public byte[] Data { get; set; }

        public static SpcData FromEbMusicData(EbMusicData ebData, int songIndex)
        {
            byte[] spc = new byte[SpcSize];

            // Load packs
            var packIndices = ebData.SongPacks[songIndex];
            foreach (var pack in packIndices.Where(i => i != null))
                LoadPack(spc, ebData.Packs[pack.Value]);

            return new SpcData { Data = spc };
        }

        private static void LoadPack(byte[] spc, Pack pack)
        {
            foreach (var chunk in pack.Chunks)
                LoadChunk(spc, chunk);
        }

        private static void LoadChunk(byte[] spc, Chunk chunk)
        {
            Array.Copy(chunk.Data, 0, spc, chunk.Pointer, chunk.Data.Length);
        }

        public SnesSample DecodeSample(int index)
        {
            int address = SpcSamplesPointer + (index * 4);
            int samplePointer = Data.ReadUShort(address);
            int brrLoopPoint = Data.ReadUShort(address + 2);

            if (samplePointer == 0xFFFF || brrLoopPoint == 0xFFFF)
                return null;

            return SnesSample.FromBrrStream(Data, samplePointer, brrLoopPoint);
        }

        public SnesInstrument ReadInstrument(int index)
        {
            int address = SpcInstrumentsPointer + (index * 6);
            return SnesInstrument.FromRom(Data, address);
        }
    }
}
