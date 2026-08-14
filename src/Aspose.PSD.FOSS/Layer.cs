using System.Drawing;

namespace Aspose.PSD.FOSS;

/// <summary>
/// Represents a single PSD layer with basic metadata used by the FOSS MVP.
/// </summary>
public class Layer
{
    /// <summary>
    /// Gets the fixed-size byte count of the layer record trailer fields.
    /// </summary>
    public const int LayerTrailerSize = 16;

    private string _name = string.Empty;
    private bool _isVisible = true;
    private byte _opacity = 255;

    /// <summary>
    /// Gets or sets the Pascal layer name stored in the layer record.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                HasMutated = true;
            }
        }
    }
    /// <summary>
    /// Gets the layer bounds in document coordinates.
    /// </summary>
    public Rectangle Bounds { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the layer is visible.
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                HasMutated = true;
            }
        }
    }
    /// <summary>
    /// Gets or sets the layer opacity in the 0-255 range.
    /// </summary>
    public byte Opacity
    {
        get => _opacity;
        set
        {
            if (_opacity != value)
            {
                _opacity = value;
                HasMutated = true;
            }
        }
    }
    /// <summary>
    /// Gets the PSD clipping value for the layer.
    /// </summary>
    public byte Clipping { get; private set; }

    /// <summary>
    /// Gets the PSD blend mode exposed by the layer record.
    /// </summary>
    public BlendMode BlendMode { get; private set; }

    /// <summary>
    /// Stores the parsed per-channel metadata from the layer record.
    /// </summary>
    internal LayerChannelInfo[] ChannelInfo { get; private set; } = [];

    /// <summary>
    /// Stores the raw layer mask subsection including its length field.
    /// </summary>
    private byte[] _layerMaskData = [];

    /// <summary>
    /// Stores the raw blending ranges subsection including its length field.
    /// </summary>
    private byte[] _blendingRangesData = [];

    /// <summary>
    /// Stores all remaining additional layer data after the Pascal layer name.
    /// </summary>
    private byte[] _additionalLayerData = [];

    /// <summary>
    /// Gets a value indicating whether the layer has a pending in-memory mutation.
    /// </summary>
    internal bool HasMutated { get; private set; }

    /// <summary>
    /// Marks the layer as mutated so the save path rewrites the minimal required structures.
    /// </summary>
    internal void MarkMutated()
    {
        HasMutated = true;
    }

    /// <summary>
    /// Loads a layer record from the current reader position.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of a layer record.</param>
    /// <param name="isLargeDocument">true for PSB-sized layer channel lengths; otherwise, false.</param>
    /// <returns>The parsed <see cref="Layer"/> instance.</returns>
    internal static Layer Load(BigEndianReader reader, bool isLargeDocument)
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
            ulong dataLength = isLargeDocument ? reader.ReadUInt64() : reader.ReadUInt32();

            channelInfoArray[i] = new LayerChannelInfo
            {
                ChannelId = channelId,
                DataLength = dataLength
            };
        }

        int signature = reader.ReadInt32();
        if (signature != 0x3842494D)
        {
            throw new PsdLoadException("Invalid layer blend mode signature. Expected '8BIM'.");
        }

        byte[] blendModeKey = reader.ReadBytes(4);
        BlendMode blendMode = ParseBlendModeKey(blendModeKey);

        byte opacity = reader.ReadByte();
        byte clipping = reader.ReadByte();
        byte flags = reader.ReadByte();
        byte filler = reader.ReadByte();

        int extraLength = reader.ReadInt32();

        string layerName = string.Empty;
        byte[] layerMaskData = [];
        byte[] blendingRangesData = [];
        byte[] additionalLayerData = [];

        if (extraLength > 0)
        {
            long extraStart = reader.Position;
            long extraEnd = extraStart + extraLength;

            try
            {
                uint layerMaskLength = reader.ReadUInt32();
                layerMaskData = new byte[4 + layerMaskLength];
                WriteUInt32BigEndian(layerMaskData, 0, layerMaskLength);
                if (layerMaskLength > 0)
                {
                    if (reader.Position + layerMaskLength > extraEnd)
                        throw new EndOfStreamException();
                    byte[] layerMaskBytes = reader.ReadBytes((int)layerMaskLength);
                    Buffer.BlockCopy(layerMaskBytes, 0, layerMaskData, 4, (int)layerMaskLength);
                }

                if (reader.Position + 4 > extraEnd)
                    throw new EndOfStreamException();
                uint blendingRangesLength = reader.ReadUInt32();
                blendingRangesData = new byte[4 + blendingRangesLength];
                WriteUInt32BigEndian(blendingRangesData, 0, blendingRangesLength);
                if (blendingRangesLength > 0)
                {
                    if (reader.Position + blendingRangesLength > extraEnd)
                        throw new EndOfStreamException();
                    byte[] blendingRangesBytes = reader.ReadBytes((int)blendingRangesLength);
                    Buffer.BlockCopy(blendingRangesBytes, 0, blendingRangesData, 4, (int)blendingRangesLength);
                }

                layerName = reader.ReadPascalString();

                long remaining = extraEnd - reader.Position;
                if (remaining > 0)
                {
                    additionalLayerData = reader.ReadBytes((int)remaining);
                }
            }
            catch (EndOfStreamException)
            {
                reader.Seek(extraEnd, SeekOrigin.Begin);
                layerName = string.Empty;
                layerMaskData = [];
                blendingRangesData = [];
                additionalLayerData = [];
            }
        }

        var bounds = new Rectangle(left, top, right - left, bottom - top);
        bool visible = (flags & 0x02) == 0;

        return new Layer
        {
            _name = layerName,
            Bounds = bounds,
            _isVisible = visible,
            _opacity = opacity,
            Clipping = clipping,
            BlendMode = blendMode,
            ChannelInfo = channelInfoArray,
            _layerMaskData = layerMaskData,
            _blendingRangesData = blendingRangesData,
            _additionalLayerData = additionalLayerData
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

    /// <summary>
    /// Writes the current layer record using PSD- or PSB-sized channel lengths.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    /// <param name="isLargeDocument">true for PSB-sized layer channel lengths; otherwise, false.</param>
    internal void Write(BigEndianWriter writer, bool isLargeDocument)
    {
        writer.Write(Bounds.Top);
        writer.Write(Bounds.Left);
        writer.Write(Bounds.Bottom);
        writer.Write(Bounds.Right);
        writer.Write((ushort)ChannelInfo.Length);

        for (int i = 0; i < ChannelInfo.Length; i++)
        {
            writer.Write(ChannelInfo[i].ChannelId);
            if (isLargeDocument)
            {
                writer.Write(ChannelInfo[i].DataLength);
            }
            else
            {
                writer.Write((uint)ChannelInfo[i].DataLength);
            }
        }

        writer.Write(0x3842494D);
        writer.Write(GetBlendModeBytes(BlendMode));
        writer.Write(Opacity);
        writer.Write(Clipping);

        int flags = 0;
        if (!IsVisible) flags |= 0x02;

        writer.Write((byte)flags);
        writer.Write((byte)0);
        int extraDataLength = _layerMaskData.Length + _blendingRangesData.Length + GetPascalStringStorageLength(Name) + _additionalLayerData.Length;
        writer.Write(extraDataLength);

        writer.Write(_layerMaskData);
        writer.Write(_blendingRangesData);
        writer.WritePascalString(Name);
        writer.Write(_additionalLayerData);
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

    /// <summary>
    /// Calculates the stored Pascal string size including the length byte and 4-byte padding.
    /// </summary>
    /// <param name="value">The layer name to measure.</param>
    /// <returns>The number of bytes required to store the name in PSD format.</returns>
    private int GetPascalStringStorageLength(string value)
    {
        int length = string.IsNullOrEmpty(value) ? 0 : System.Text.Encoding.ASCII.GetByteCount(value);
        return 1 + length + ((4 - ((length + 1) % 4)) % 4);
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer into a byte buffer in big-endian byte order.
    /// </summary>
    /// <param name="buffer">The target buffer.</param>
    /// <param name="offset">The offset where the value should be written.</param>
    /// <param name="value">The value to encode.</param>
    private static void WriteUInt32BigEndian(byte[] buffer, int offset, uint value)
    {
        buffer[offset] = (byte)((value >> 24) & 0xFF);
        buffer[offset + 1] = (byte)((value >> 16) & 0xFF);
        buffer[offset + 2] = (byte)((value >> 8) & 0xFF);
        buffer[offset + 3] = (byte)(value & 0xFF);
    }

    /// <summary>
    /// Stores one channel metadata entry from a layer record.
    /// </summary>
    internal struct LayerChannelInfo
    {
        /// <summary>
        /// Gets or sets the PSD channel identifier.
        /// </summary>
        public short ChannelId;

        /// <summary>
        /// Gets or sets the declared byte length of the channel data payload.
        /// </summary>
        public ulong DataLength;
    }
}
