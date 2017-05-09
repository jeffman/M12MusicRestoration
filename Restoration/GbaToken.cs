using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration
{
    public sealed class GbaToken
    {
        #region Maps

        private static readonly GbaTokenType[] TokenMap =
        {
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Repeat,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.Rest,
            GbaTokenType.End,
            GbaTokenType.Jump,
            GbaTokenType.Call,
            GbaTokenType.Return,
            GbaTokenType.CallAndRepeat,
            GbaTokenType.Invalid,
            GbaTokenType.Invalid,
            GbaTokenType.Invalid,
            GbaTokenType.ConditionalJump,
            GbaTokenType.Priority,
            GbaTokenType.Tempo,
            GbaTokenType.Transpose,
            GbaTokenType.Instrument,
            GbaTokenType.Volume,
            GbaTokenType.Panning,
            GbaTokenType.PitchBendValue,
            GbaTokenType.PitchBendRange,
            GbaTokenType.LfoSpeed,
            GbaTokenType.LfoDelay,
            GbaTokenType.LfoDepth,
            GbaTokenType.LfoType,
            GbaTokenType.Invalid,
            GbaTokenType.Invalid,
            GbaTokenType.Detune,
            GbaTokenType.Invalid,
            GbaTokenType.Invalid,
            GbaTokenType.Invalid,
            GbaTokenType.Invalid,
            GbaTokenType.Echo,
            GbaTokenType.NoteOff,
            GbaTokenType.NoteOn,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout,
            GbaTokenType.NoteOnWithTimeout
        };

        #endregion

        private static Dictionary<GbaTokenType, byte> SingleCodeLookup;

        public GbaTokenType Type { get; private set; }
        public byte Code { get; private set; }
        public byte[] Args { get; private set; }

        static GbaToken()
        {
            SingleCodeLookup = TokenMap.Select((t, i) => new { Index = i, Token = t })
                .Where(t => TokenMap.Count(v => v == t.Token) == 1)
                .ToDictionary(t => t.Token, t => (byte)t.Index);
        }

        public static GbaToken Create(GbaTokenType type, params byte[] args)
        {
            return new GbaToken { Type = type, Code = SingleCodeLookup[type], Args = args };
        }

        public static GbaToken Create(byte code, params byte[] args)
        {
            return new GbaToken { Type = TokenMap[code], Code = code, Args = args };
        }

        public static GbaToken CreateRest(byte time)
        {
            if (time > 48)
                throw new Exception("Time must be between 0 and 48, inclusive");

            return Create((byte)(time + 128));
        }

        public static GbaToken CreateNoteOn(byte key)
        {
            if (key > 127)
                throw new Exception("Key must be between 0 and 127, inclusive");

            return Create(GbaTokenType.NoteOn, key);
        }

        public static GbaToken CreateNoteOn(byte key, byte velocity)
        {
            if (key > 127)
                throw new Exception("Key must be between 0 and 127, inclusive");

            return Create(GbaTokenType.NoteOn, key, velocity);
        }

        public static GbaToken CreateNoteOnAutoOff(byte key, byte length, byte velocity)
        {
            if (key > 127)
                throw new Exception("Key must be between 0 and 127, inclusive");

            return Create((byte)(length + 0xCF), key, velocity);
        }

        public static GbaToken CreateNoteOnAutoOff(byte key, byte length)
        {
            if (key > 127)
                throw new Exception("Key must be between 0 and 127, inclusive");

            return Create((byte)(length + 0xCF), key);
        }

        public static GbaToken CreateNoteOff(byte key)
        {
            if (key > 127)
                throw new Exception("Key must be between 0 and 127, inclusive");

            return Create(GbaTokenType.NoteOff, key);
        }

        public override string ToString()
        {
            return $"{Type}: [{Code:X2}{String.Concat(Args.Select(a => " " + a.ToString("X2")))}]";
        }

        public int Serialize(byte[] rom, int address)
        {
            rom[address++] = Code;
            foreach (byte arg in Args)
                rom[address++] = arg;
            return address;
        }
    }

    public enum GbaTokenType
    {
        Repeat,
        Rest,
        End,
        Jump,
        Call,
        Return,
        CallAndRepeat,
        ConditionalJump,
        Priority,
        Tempo,
        Transpose,
        Instrument,
        Volume,
        Panning,
        PitchBendValue,
        PitchBendRange,
        LfoSpeed,
        LfoDelay,
        LfoDepth,
        LfoType,
        Unknown,
        Detune,
        Echo,
        NoteOff,
        NoteOn,
        NoteOnWithTimeout,
        Invalid
    }
}
