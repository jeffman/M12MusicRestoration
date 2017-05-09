using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Restoration
{
    class Program
    {
        static readonly int[] snesHeaderedRomSizes =
        {
            0x300200,
            0x400200,
            0x600200
        };

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Restoration.exe <ebrom.smc> <m12rom.gba>");
                return;
            }

            var ebRom = LoadEbRom(args[0]);
            var m12Rom = File.ReadAllBytes(args[1]);

            var ebMusicData = new EbMusicData();
            ebMusicData.LoadFromRom(ebRom);

            int songIndex = 0xae;

            var music = SnesMusic.FromEbData(ebMusicData, songIndex);
            var gbaMusic = GbaMusic.FromSnesMusic(music);

            foreach (var track in gbaMusic.Song.Tracks)
            {
                track.Tokens.Insert(0, GbaToken.Create(GbaTokenType.Transpose, 0));
                track.Tokens.Insert(0, GbaToken.Create(GbaTokenType.Volume, 0x70));
                track.Tokens.Insert(0, GbaToken.Create(GbaTokenType.Panning, 0x40));

                if (track.Tokens.Last().Type != GbaTokenType.End)
                    track.Tokens.Add(GbaToken.Create(GbaTokenType.End));
            }

            int musicPointer = 0xB30000;
            gbaMusic.Serialize(m12Rom, musicPointer);

            songIndex = 0xAE; // music title screen
            m12Rom.WriteGbaPointer(0x10B530 + (songIndex + 1) * 8, musicPointer);

            File.WriteAllBytes(args[1] + ".out.gba", m12Rom);
        }

        static byte[] LoadEbRom(string fileName)
        {
            byte[] rom = File.ReadAllBytes(fileName);
            if (HasSnesHeader(rom))
                return rom.Skip(0x200).ToArray();
            return rom;
        }

        static bool HasSnesHeader(byte[] rom)
        {
            return snesHeaderedRomSizes.Contains(rom.Length);
        }
    }
}
