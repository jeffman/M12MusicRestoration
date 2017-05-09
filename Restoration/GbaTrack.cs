using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class GbaTrack
    {
        public List<GbaToken> Tokens { get; set; }

        public GbaTrack()
        {
            Tokens = new List<Restoration.GbaToken>();
        }

        public int Serialize(byte[] rom, int address)
        {
            foreach (var token in Tokens)
                address = token.Serialize(rom, address);
            return address;
        }
    }
}
