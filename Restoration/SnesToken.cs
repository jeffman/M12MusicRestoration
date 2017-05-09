using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class SnesToken
    {
        private static readonly SnesTokenType[] TokenMap = new SnesTokenType[256];
        private static readonly Dictionary<SnesTokenType, int> TokenArgCount
            = new Dictionary<SnesTokenType, int>();

        public byte Code { get; private set; }
        public SnesTokenType Type { get; private set; }
        public byte[] Args { get; private set; }

        static SnesToken()
        {
            TokenMap[0] = SnesTokenType.End;

            for (byte b = 1; b <= 0x7F; b++)
                TokenMap[b] = SnesTokenType.NoteLength;

            for (byte b = 0x80; b <= 0xC7; b++)
                TokenMap[b] = SnesTokenType.PlayNote;

            TokenMap[0xC8] = SnesTokenType.ContinueNote;
            TokenMap[0xC9] = SnesTokenType.Rest;

            for (byte b = 0xCA; b <= 0xDF; b++)
                TokenMap[b] = SnesTokenType.SetInstrumentPlayC4;

            TokenMap[0xE0] = SnesTokenType.SetInstrument;
            TokenMap[0xE1] = SnesTokenType.Panning;
            TokenMap[0xE2] = SnesTokenType.SlidePanning;
            TokenMap[0xE3] = SnesTokenType.VibratoOn;
            TokenMap[0xE4] = SnesTokenType.VibratoOff;
            TokenMap[0xE5] = SnesTokenType.GlobalVolume;
            TokenMap[0xE6] = SnesTokenType.SlideGlobalVolume;
            TokenMap[0xE7] = SnesTokenType.Tempo;
            TokenMap[0xE8] = SnesTokenType.SlideTempo;
            TokenMap[0xE9] = SnesTokenType.GlobalTranspose;
            TokenMap[0xEA] = SnesTokenType.ChannelTranspose;
            TokenMap[0xEB] = SnesTokenType.TremoloOn;
            TokenMap[0xEC] = SnesTokenType.TremoloOff;
            TokenMap[0xED] = SnesTokenType.ChannelVolume;
            TokenMap[0xEE] = SnesTokenType.SlideChannelVolume;
            TokenMap[0xEF] = SnesTokenType.Call;
            TokenMap[0xF0] = SnesTokenType.VibratoTime;
            TokenMap[0xF1] = SnesTokenType.PortamentoOn1;
            TokenMap[0xF2] = SnesTokenType.PortamentoOn2;
            TokenMap[0xF3] = SnesTokenType.PortamentoOff;
            TokenMap[0xF4] = SnesTokenType.Finetune;
            TokenMap[0xF5] = SnesTokenType.EchoOn;
            TokenMap[0xF6] = SnesTokenType.EchoOff;
            TokenMap[0xF7] = SnesTokenType.EchoSettings;
            TokenMap[0xF8] = SnesTokenType.SlideEchoVolume;
            TokenMap[0xF9] = SnesTokenType.NotePortamento;
            TokenMap[0xFA] = SnesTokenType.SetBaseInstrument;
            TokenMap[0xFB] = SnesTokenType.Nothing;
            TokenMap[0xFC] = SnesTokenType.Mute;
            TokenMap[0xFD] = SnesTokenType.FastForwardOn;
            TokenMap[0xFE] = SnesTokenType.FastForwardOff;
            TokenMap[0xFF] = SnesTokenType.Invalid;

            var argCountLookup = new SnesTokenType[][]
            {
                new SnesTokenType[]
                {
                    SnesTokenType.End,
                    SnesTokenType.NoteLength,
                    SnesTokenType.PlayNote,
                    SnesTokenType.ContinueNote,
                    SnesTokenType.Rest,
                    SnesTokenType.SetInstrumentPlayC4,
                    SnesTokenType.VibratoOff,
                    SnesTokenType.TremoloOff,
                    SnesTokenType.PortamentoOff,
                    SnesTokenType.EchoOff,
                    SnesTokenType.Mute,
                    SnesTokenType.FastForwardOn,
                    SnesTokenType.FastForwardOff,
                    SnesTokenType.Invalid
                },
                new SnesTokenType[]
                {
                    SnesTokenType.SetInstrument,
                    SnesTokenType.Panning,
                    SnesTokenType.GlobalVolume,
                    SnesTokenType.Tempo,
                    SnesTokenType.GlobalTranspose,
                    SnesTokenType.ChannelTranspose,
                    SnesTokenType.ChannelVolume,
                    SnesTokenType.VibratoTime,
                    SnesTokenType.Finetune,
                    SnesTokenType.SetBaseInstrument
                },
                new SnesTokenType[]
                {
                    SnesTokenType.SlidePanning,
                    SnesTokenType.SlideGlobalVolume,
                    SnesTokenType.SlideTempo,
                    SnesTokenType.SlideChannelVolume,
                    SnesTokenType.Nothing
                },
                new SnesTokenType[]
                {
                    SnesTokenType.VibratoOn,
                    SnesTokenType.TremoloOn,
                    SnesTokenType.Call,
                    SnesTokenType.PortamentoOn1,
                    SnesTokenType.PortamentoOn2,
                    SnesTokenType.EchoOn,
                    SnesTokenType.EchoSettings,
                    SnesTokenType.SlideEchoVolume,
                    SnesTokenType.NotePortamento
                }
            };

            for (int length = 0; length < argCountLookup.Length; length++)
            {
                foreach (var token in argCountLookup[length])
                    TokenArgCount.Add(token, length);
            }
        }

        public static SnesToken FromRom(byte[] rom, int address)
        {
            byte code = rom[address++];
            var tokenType = TokenMap[code];

            byte next = rom[address];
            byte[] args;

            if (tokenType == SnesTokenType.NoteLength && next >= 1 && next <= 0x7F)
            {
                address++;
                args = new byte[] { next };
            }
            else
            {
                args = rom.ReadBytes(address, TokenArgCount[tokenType]);
            }

            return new SnesToken { Code = code, Type = tokenType, Args = args };
        }

        public override string ToString()
        {
            return $"{Type}: [{Code:X2}{String.Concat(Args.Select(a => " " + a.ToString("X2")))}]";
        }
    }

    public enum SnesTokenType
    {
        End,
        NoteLength,
        PlayNote,
        ContinueNote,
        Rest,
        SetInstrumentPlayC4,
        SetInstrument,
        Panning,
        SlidePanning,
        VibratoOn,
        VibratoOff,
        GlobalVolume,
        SlideGlobalVolume,
        Tempo,
        SlideTempo,
        GlobalTranspose,
        ChannelTranspose,
        TremoloOn,
        TremoloOff,
        ChannelVolume,
        SlideChannelVolume,
        Call,
        VibratoTime,
        PortamentoOn1,
        PortamentoOn2,
        PortamentoOff,
        Finetune,
        EchoOn,
        EchoOff,
        EchoSettings,
        SlideEchoVolume,
        NotePortamento,
        SetBaseInstrument,
        Nothing,
        Mute,
        FastForwardOn,
        FastForwardOff,
        Invalid
    }
}
