using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class GbaSong
    {
        private static readonly Dictionary<byte, byte> NoteLengthMap
            = new Dictionary<byte, byte>();

        public List<GbaTrack> Tracks { get; set; }

        static GbaSong()
        {
            for (byte b = 1; b <= 24; b++)
                NoteLengthMap.Add(b, b);

            NoteLengthMap.Add(28, 25);
            NoteLengthMap.Add(30, 26);
            NoteLengthMap.Add(32, 27);
            NoteLengthMap.Add(36, 28);
            NoteLengthMap.Add(40, 29);
            NoteLengthMap.Add(42, 30);
            NoteLengthMap.Add(44, 31);
            NoteLengthMap.Add(48, 32);
            NoteLengthMap.Add(52, 33);
            NoteLengthMap.Add(54, 34);
            NoteLengthMap.Add(56, 35);
            NoteLengthMap.Add(60, 36);
            NoteLengthMap.Add(64, 37);
            NoteLengthMap.Add(66, 38);
            NoteLengthMap.Add(68, 39);
            NoteLengthMap.Add(72, 40);
            NoteLengthMap.Add(76, 41);
            NoteLengthMap.Add(78, 42);
            NoteLengthMap.Add(80, 43);
            NoteLengthMap.Add(84, 44);
            NoteLengthMap.Add(88, 45);
            NoteLengthMap.Add(90, 46);
            NoteLengthMap.Add(92, 47);
            NoteLengthMap.Add(96, 48);
        }

        public static GbaSong FromSnesSong(SnesSong song, int group)
        {
            var snesTracks = song.TrackGroups[group];
            var gbaTracks = new GbaTrack[snesTracks.Length];

            // Initialize all tracks first so that we can insert global tokens
            for (int i = 0; i < gbaTracks.Length; i++)
                gbaTracks[i] = new GbaTrack();

            int baseInstrument = 0;

            for (int i = 0; i < gbaTracks.Length; i++)
            {
                if (snesTracks[i] == null)
                {
                    gbaTracks[i] = null;
                    continue;
                }

                ConvertSnesTrack(snesTracks[i], gbaTracks[i], ref baseInstrument, t =>
                {
                    foreach (var gbaTrack in gbaTracks)
                        gbaTrack.Tokens.Insert(0, t);
                });
            }

            return new GbaSong { Tracks = gbaTracks.Where(t => t != null).ToList() };
        }

        public int[] Serialize(byte[] rom, int address)
        {
            var trackPointers = new int[Tracks.Count];
            for (int i = 0; i < Tracks.Count; i++)
            {
                trackPointers[i] = address;
                address = Tracks[i].Serialize(rom, address);
            }
            return trackPointers;
        }

        private static void ConvertSnesTrack(SnesTrack snesTrack, GbaTrack gbaTrack,
            ref int baseInstrument, Action<GbaToken> globalCallback)
        {
            if (snesTrack == null)
                return;

            Action<GbaToken> Add = gbaTrack.Tokens.Add;

            byte noteLength = 0;
            byte noteKey = 0;

            for (int i = 0; i < snesTrack.Tokens.Count; i++)
            {
                var snesToken = snesTrack.Tokens[i];

                switch (snesToken.Type)
                {
                    case SnesTokenType.SetBaseInstrument:
                        baseInstrument = snesToken.Args[0];
                        break;

                    case SnesTokenType.Tempo:
                        Add(GbaToken.Create(GbaTokenType.Tempo, (byte)(snesToken.Args[0] * 19 / 8)));
                        break;

                    case SnesTokenType.SetInstrument:
                        {
                            byte arg = snesToken.Args[0];
                            byte instrument = (byte)((arg >= 0xCA) ? (arg + baseInstrument - 0xCA) : arg);
                            Add(GbaToken.Create(GbaTokenType.Instrument, instrument));
                        }
                        break;

                    case SnesTokenType.NoteLength:
                        noteLength = NoteLengthMap[snesToken.Code];
                        break;

                    case SnesTokenType.PlayNote:
                        {
                            byte key = (byte)(snesToken.Code - 0x80);
                            noteKey = (byte)(key + 0);
                            Add(GbaToken.CreateNoteOn(noteKey, 96));
                            Add(GbaToken.CreateRest(noteLength));
                            Add(GbaToken.CreateNoteOff(noteKey));
                        }
                        break;

                    case SnesTokenType.Rest:
                        Add(GbaToken.CreateNoteOff(noteKey));
                        Add(GbaToken.CreateRest(noteLength));
                        break;

                    case SnesTokenType.End:
                        Add(GbaToken.Create(GbaTokenType.End));
                        break;

                    case SnesTokenType.ContinueNote:
                        Add(GbaToken.CreateNoteOn(noteKey, 96));
                        Add(GbaToken.CreateRest(noteLength));
                        Add(GbaToken.CreateNoteOff(noteKey));
                        break;

                    //case SnesTokenType.GlobalVolume:
                    //    globalCallback(GbaToken.Create(GbaTokenType.Volume, snesToken.Args[0]));
                    //    goto case SnesTokenType.ChannelVolume;

                    case SnesTokenType.ChannelVolume:
                        Add(GbaToken.Create(GbaTokenType.Volume, (byte)(snesToken.Args[0] * 3 / 4)));
                        break;

                    case SnesTokenType.Panning:
                        // SNES: 0 = right, 0x14 = left
                        // GBA: 0 = left, 0x7F = right
                        int panning = 0x14 - snesToken.Args[0];
                        panning *= 0x7F / 0x14;
                        Add(GbaToken.Create(GbaTokenType.Panning, (byte)panning));
                        break;
                }
            }
        }
    }
}
