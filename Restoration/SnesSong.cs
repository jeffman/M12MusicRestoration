using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SnesSong
    {
        public List<SnesTrack[]> TrackGroups { get; private set; }
        public int LoopPoint { get; set; }

        public static SnesSong FromRom(byte[] rom, int address)
        {
            int loopIndex;
            var trackGroups = ReadTrackGroups(rom, address, out loopIndex);

            return new SnesSong { TrackGroups = trackGroups, LoopPoint = loopIndex };
        }

        private static List<SnesTrack[]> ReadTrackGroups(byte[] rom, int address, out int loopIndex)
        {
            int baseAddress = address;
            loopIndex = -1;
            var trackGroups = new List<SnesTrack[]>();

            int groupPointer;
            while ((groupPointer = rom.ReadUShort(address)) != 0)
            {
                if (groupPointer == 0xFF)
                {
                    int jumpPointer = rom.ReadUShort(address + 2);
                    loopIndex = (jumpPointer - baseAddress) / 2;
                    address += 2;
                }
                else
                {
                    trackGroups.Add(ReadTrackGroup(rom, groupPointer));
                }
                address += 2;
            }

            return trackGroups;
        }

        private static SnesTrack[] ReadTrackGroup(byte[] rom, int address)
        {
            int[] trackPointers = new int[8];
            for (int i = 0; i < 8; i++)
            {
                trackPointers[i] = rom.ReadUShort(address + (i * 2));
            }

            return SnesTrack.ReadTracksFromRom(rom, trackPointers);
        }
    }
}
