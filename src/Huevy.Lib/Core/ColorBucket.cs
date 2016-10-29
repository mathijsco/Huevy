using System;
using System.Collections.Generic;

namespace Huevy.Lib.Core
{
    //public sealed class ColorBucket
    //{
    //    private readonly Dictionary<TinyColor, int> _innerBucket;
    //    //private static readonly Func<byte, byte> ColorPicker = LooseBitjes;

    //    public ColorBucket()
    //    {
    //        _innerBucket = new Dictionary<TinyColor, int>(300);
    //    }

    //    internal IReadOnlyDictionary<TinyColor, int> Dictionary
    //    {
    //        get { return _innerBucket; }
    //    }

    //    public void Add(TinyColor color)
    //    {
    //        //var color = (red << 16) | (green << 8) | blue;
    //        //var color = (FindClosest32Byte(red) << 16) | (FindClosest32Byte(green) << 8) | FindClosest32Byte(blue);

    //        //var color = Color.FromArgb(
    //        //    FindClosest32Byte(red),
    //        //    FindClosest32Byte(green),
    //        //    FindClosest32Byte(blue)
    //        //);

    //        int amount;
    //        if (_innerBucket.TryGetValue(color, out amount))
    //            _innerBucket[color] = amount + 1;
    //        else
    //            _innerBucket.Add(color, 1);
    //    }

    //    // No adjustment
    //    //private static byte Direct(byte input)
    //    //{
    //    //    return input;
    //    //}
        
    //    //// No adjustment
    //    //private static byte LooseBitjes(byte input)
    //    //{
    //    //    // 1110 0000

    //    //    // < 0010 0000
    //    //    if (input < 0x10) return 0x00;
    //    //    // > 1110 0000
    //    //    //if (input >= 0xE0) return 0xFF;
    //    //    if (input >= 0xF8) return 0xFF;

    //    //    // ###* ****
    //    //    return (byte)(input & 0xF0);
    //    //}

    //    //// Round on 40
    //    //private static byte FindClosest40Byte(byte input)
    //    //{
    //    //    const double rounding = 40d;
    //    //    return (byte)(Math.Round(input / rounding) * rounding);
    //    //}

    //    //// Round on 32
    //    //private static byte FindClosest32Byte(byte input)
    //    //{
    //    //    // 1110 0000

    //    //    // < 0010 0000
    //    //    if (input < 0x20) return 0x00;
    //    //    // > 1110 0000
    //    //    //if (input >= 0xE0) return 0xFF;
    //    //    if (input >= 0xF0) return 0xFF;

    //    //    // ###* ****
    //    //    return (byte)(input & 0xE0);
    //    //}

    //    //// Round on 64
    //    //private static byte FindClosest64Byte(byte input)
    //    //{
    //    //    // 1110 0000
    //    //    // Trim bytes before 32: & 0xE0

    //    //    // < 0100 0000
    //    //    if (input < 0x40) return 0x00;
    //    //    // > 1100 0000
    //    //    if (input >= 0xC0) return 0xFF;

    //    //    // ##** ****
    //    //    return (byte)(input & 0xC0);
    //    //}
    //}
}
