using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class EbMusicData
    {
        const int PackTable = 0x4F947;
        const int PackCount = 0xA9;

        const int SongPackTable = 0x4F70A;
        const int SongCount = 0xBF;

        const int SongSpcPointerTable = 0x26298C;

        public int[] PackPointers { get; private set; }
        public int?[][] SongPacks { get; private set; }
        public int[] SongSpcPointers { get; private set; }
        public Pack[] Packs { get; private set; }

        public void LoadFromRom(byte[] rom)
        {
            ReadPackPointers(rom);
            ReadSongPacks(rom);
            ReadSongSpcPointers(rom);
            ReadPacks(rom);
        }

        private void ReadPackPointers(byte[] rom)
        {
            PackPointers = new int[PackCount];
            for (int i = 0; i < PackCount; i++)
            {
                PackPointers[i] = rom.ReadHLMPointer(PackTable + (i * 3));
            }
        }

        private void ReadSongPacks(byte[] rom)
        {
            SongPacks = new int?[SongCount][];
            for (int i = 0; i < SongCount; i++)
            {
                SongPacks[i] = new int?[3];
                for (int j = 0; j < 3; j++)
                {
                    byte pack = rom[SongPackTable + (i * 3) + j];
                    SongPacks[i][j] = (pack == 0xFF) ? (int?)null : pack;
                }
            }
        }

        private void ReadSongSpcPointers(byte[] rom)
        {
            SongSpcPointers = new int[SongCount];
            for (int i = 0; i < SongCount; i++)
            {
                SongSpcPointers[i] = rom.ReadUShort(SongSpcPointerTable + (i * 2));
            }
        }

        private void ReadPacks(byte[] rom)
        {
            Packs = new Pack[PackCount];
            for (int i = 0; i < PackCount; i++)
            {
                Packs[i] = Pack.FromRom(rom, PackPointers[i]);
            }
        }
    }
}
