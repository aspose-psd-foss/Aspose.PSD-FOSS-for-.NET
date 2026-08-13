using System.Drawing;

namespace Aspose.PSD.FOSS;

public class Layer
{
    public const int LayerTrailerSize = 16;

    public string Name { get; set; } = string.Empty;
    public Rectangle Bounds { get; private set; }
    public bool IsVisible { get; set; } = true;
    public byte Opacity { get; set; } = 255;
    public byte Clipping { get; private set; }
    public BlendMode BlendMode { get; private set; }

    internal LayerChannelInfo[] ChannelInfo { get; private set; } = [];
    private byte[] _extraData = [];

    internal static Layer Load(BigEndianReader reader, int channelCount)
    {
        int top = reader.ReadInt32();
        int left = reader.ReadInt32();
        int bottom = reader.ReadInt32();
        int right = reader.ReadInt32();

        ushort actualChannelCount = reader.ReadUInt16();

        var channelInfoArray = new LayerChannelInfo[actualChannelCount];
        
        for (int i = 0; i < actualChannelCount; i++)
        {
            short channelId = reader.ReadInt16();
            uint dataLength = reader.ReadUInt32();

            channelInfoArray[i] = new LayerChannelInfo
            {
                ChannelId = channelId,
                DataLength = dataLength
            };
        }

        int signature = reader.ReadInt32();
        if (signature != 0x3842494D)
        {
        }

        byte[] blendModeKey = reader.ReadBytes(4);
        BlendMode blendMode = ParseBlendModeKey(blendModeKey);

        byte opacity = reader.ReadByte();
        byte clipping = reader.ReadByte();
        byte flags = reader.ReadByte();
        byte filler = reader.ReadByte();

        int extraLength = reader.ReadInt32();

        string layerName = string.Empty;
        byte[] extraData = [];

        if (extraLength > 0)
        {
            long extraStart = reader.Position;
            long extraEnd = extraStart + extraLength;

            try
            {
                uint layerMaskLength = reader.ReadUInt32();
                if (layerMaskLength > 0)
                {
                    if (reader.Position + layerMaskLength > extraEnd)
                        throw new EndOfStreamException();
                    reader.Skip((int)layerMaskLength);
                }

                if (reader.Position + 8 > extraEnd)
                    throw new EndOfStreamException();
                reader.Skip(8);

                uint blendingRangesLength = reader.ReadUInt32();
                if (blendingRangesLength > 0)
                {
                    if (reader.Position + blendingRangesLength > extraEnd)
                        throw new EndOfStreamException();
                    reader.Skip((int)blendingRangesLength);
                }

                layerName = reader.ReadPascalString();

                long remaining = extraEnd - reader.Position;
                if (remaining > 0)
                {
                    extraData = reader.ReadBytes((int)remaining);
                }
            }
            catch (EndOfStreamException)
            {
                reader.Seek(extraEnd, SeekOrigin.Begin);
                layerName = string.Empty;
                extraData = [];
            }
        }

        var bounds = new Rectangle(left, top, right - left, bottom - top);
        bool visible = (flags & 0x02) == 0;

        return new Layer
        {
            Name = layerName,
            Bounds = bounds,
            IsVisible = visible,
            Opacity = opacity,
            Clipping = clipping,
            BlendMode = blendMode,
            ChannelInfo = channelInfoArray,
            _extraData = extraData
        };
    }

    private static BlendMode ParseBlendModeKey(byte[] key)
    {
        if (key.Length < 4) return BlendMode.Normal;

        string modeKey = System.Text.Encoding.ASCII.GetString(key);
        return modeKey switch
        {
            "norm" => BlendMode.Normal,
            "mul " => BlendMode.Multiply,
            "scrn" => BlendMode.Screen,
            "over" => BlendMode.Overlay,
            "dark" => BlendMode.Darken,
            "lite" => BlendMode.Lighten,
            "div " => BlendMode.ColorDodge,
            "burn" => BlendMode.ColorBurn,
            "hlit" => BlendMode.HardLight,
            "slit" => BlendMode.SoftLight,
            "diff" => BlendMode.Difference,
            "smud" => BlendMode.Exclusion,
            "hue " => BlendMode.Hue,
            "sat " => BlendMode.Saturation,
            "colr" => BlendMode.Color,
            "lum " => BlendMode.Luminosity,
            _ => BlendMode.Normal
        };
    }

    internal void Write(BigEndianWriter writer, int channelCount)
    {
        writer.Write(Bounds.Top);
        writer.Write(Bounds.Left);
        writer.Write(Bounds.Bottom);
        writer.Write(Bounds.Right);
        writer.Write((ushort)channelCount);

        for (int i = 0; i < channelCount; i++)
        {
            writer.Write((short)i);
            writer.Write((uint)0);
        }

        writer.Write(0x3842494D);
        writer.Write(GetBlendModeBytes(BlendMode));
        writer.Write(Opacity);
        writer.Write(Clipping);

        int flags = 0;
        if (!IsVisible) flags |= 0x02;

        writer.Write(flags);
        writer.Write((int)_extraData.Length);

        writer.Write((uint)0);
        writer.Write(new byte[8]);

        writer.WritePascalString(Name);
        writer.Write(_extraData);
    }

    private byte[] GetBlendModeBytes(BlendMode mode)
    {
        string key = mode switch
        {
            BlendMode.Normal => "norm",
            BlendMode.Multiply => "mul ",
            BlendMode.Screen => "scrn",
            BlendMode.Overlay => "over",
            BlendMode.Darken => "dark",
            BlendMode.Lighten => "lite",
            BlendMode.ColorDodge => "div ",
            BlendMode.ColorBurn => "burn",
            BlendMode.HardLight => "hlit",
            BlendMode.SoftLight => "slit",
            BlendMode.Difference => "diff",
            BlendMode.Exclusion => "smud",
            BlendMode.Hue => "hue ",
            BlendMode.Saturation => "sat ",
            BlendMode.Color => "colr",
            BlendMode.Luminosity => "lum ",
            _ => "norm"
        };
        return System.Text.Encoding.ASCII.GetBytes(key);
    }

    internal struct LayerChannelInfo
    {
        public short ChannelId;
        public uint DataLength;
    }
}
