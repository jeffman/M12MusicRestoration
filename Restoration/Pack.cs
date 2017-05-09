using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class Pack
    {
        public List<Chunk> Chunks { get; set; }

        public static Pack FromRom(byte[] rom, int address)
        {
            // Read chunks until a 0-length chunk is encountered
            var chunks = new List<Chunk>();

            Chunk chunk;
            while ((chunk = Chunk.FromRom(rom, address)) != null)
            {
                chunks.Add(chunk);
                address += chunk.Data.Length + 4;
            }

            return new Pack { Chunks = chunks };
        }

        public override string ToString()
        {
            return $"Count: {Chunks.Count}, Total length: {Chunks.Sum(ch => ch.Data.Length)}";
        }
    }
}
