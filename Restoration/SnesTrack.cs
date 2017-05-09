using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SnesTrack
    {
        public List<SnesToken> Tokens { get; private set; }

        public static SnesTrack[] ReadTracksFromRom(byte[] rom, int[] trackPointers)
        {
            var tracks = new SnesTrack[8];

            for (int i = 0; i < 8; i++)
            {
                int address = trackPointers[i];
                if (address == 0)
                    continue;

                var tokens = new List<SnesToken>();
                SnesToken token;

                do
                {
                    token = SnesToken.FromRom(rom, address);
                    tokens.Add(token);
                    address += token.Args.Length + 1;
                } while (token.Type != SnesTokenType.End && !trackPointers.Contains(address));

                tracks[i] = new SnesTrack { Tokens = tokens };
            }

            return tracks;
        }
    }
}
