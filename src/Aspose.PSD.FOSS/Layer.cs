using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using System.Linq;

namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Represents a single PSD layer with basic metadata used by the FOSS library.
/// </summary>
public class Layer
{
    /// <summary>
    /// Gets the fixed-size byte count of the layer record trailer fields.
    /// </summary>
    public const int LayerTrailerSize = 16;

    /// <summary>
    /// Stores the current layer name.
    /// </summary>
    private string _name = string.Empty;

    /// <summary>
    /// Stores the current layer visibility flag.
    /// </summary>
    private bool _isVisible = true;

    /// <summary>
    /// Stores the current layer opacity value.
    /// </summary>
    private byte _opacity = 255;

    /// <summary>
    /// Stores the original PSD layer flags byte.
    /// </summary>
    private byte _flags;

    /// <summary>
    /// Stores the original 4-byte PSD blend mode key.
    /// </summary>
    private string _blendModeKey = LayerBlendModeMapper.NormalBlendModeKey;

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
    /// Gets the layer rectangle in PSD document coordinates.
    /// </summary>
    public Rectangle Bounds
    {
        get => _bounds;
        set
        {
            if (_bounds != value)
            {
                _bounds = value;
                HasMutated = true;
            }
        }
    }

    /// <summary>
    /// Stores the current layer bounds in document coordinates.
    /// </summary>
    private Rectangle _bounds;

    /// <summary>
    /// Gets the layer width in pixels.
    /// </summary>
    public int Width => Bounds.Width;

    /// <summary>
    /// Gets the layer height in pixels.
    /// </summary>
    public int Height => Bounds.Height;

    /// <summary>
    /// Gets the top edge of the layer bounds.
    /// </summary>
    public int Top
    {
        get => Bounds.Top;
        set
        {
            if (Bounds.Top != value)
            {
                Bounds = Rectangle.FromLTRB(Bounds.Left, value, Bounds.Right, Bounds.Bottom);
            }
        }
    }

    /// <summary>
    /// Gets the left edge of the layer bounds.
    /// </summary>
    public int Left
    {
        get => Bounds.Left;
        set
        {
            if (Bounds.Left != value)
            {
                Bounds = Rectangle.FromLTRB(value, Bounds.Top, Bounds.Right, Bounds.Bottom);
            }
        }
    }

    /// <summary>
    /// Gets the bottom edge of the layer bounds.
    /// </summary>
    public int Bottom
    {
        get => Bounds.Bottom;
        set
        {
            if (Bounds.Bottom != value)
            {
                Bounds = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, Bounds.Right, value);
            }
        }
    }

    /// <summary>
    /// Gets the right edge of the layer bounds.
    /// </summary>
    public int Right
    {
        get => Bounds.Right;
        set
        {
            if (Bounds.Right != value)
            {
                Bounds = Rectangle.FromLTRB(Bounds.Left, Bounds.Top, value, Bounds.Bottom);
            }
        }
    }

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
    public byte Clipping
    {
        get => _clipping;
        set
        {
            if (_clipping != value)
            {
                _clipping = value;
                HasMutated = true;
            }
        }
    }

    /// <summary>
    /// Stores the PSD clipping value for the layer.
    /// </summary>
    private byte _clipping;

    /// <summary>
    /// Gets the PSD blend mode exposed by the layer record.
    /// </summary>
    public BlendMode BlendMode
    {
        get => _blendMode;
        set
        {
            if (_blendMode != value)
            {
                _blendMode = value;
                _blendModeKey = LayerBlendModeMapper.GetBlendModeKey(value);
                HasMutated = true;
            }
        }
    }

    /// <summary>
    /// Stores the PSD blend mode exposed by the layer record.
    /// </summary>
    private BlendMode _blendMode;

    /// <summary>
    /// Gets or sets the layer blend mode key.
    /// </summary>
    public BlendMode BlendModeKey
    {
        get => _blendMode;
        set => BlendMode = value;
    }

    /// <summary>
    /// Gets the original raw 4-byte PSD blend mode key for diagnostics and raw-preserve verification.
    /// </summary>
    public string RawBlendModeKey => _blendModeKey;

    /// <summary>
    /// Gets the number of parsed channel records in the layer.
    /// </summary>
    public int ChannelCount => ChannelInfo.Length;

    /// <summary>
    /// Gets a read-only summary of the parsed layer channel records.
    /// </summary>
    internal IReadOnlyList<PsdLayerChannelInfo> Channels => ChannelInfo.Select(channel => new PsdLayerChannelInfo(channel.ChannelId, channel.DataLength)).ToArray();

    /// <summary>
    /// Gets a value indicating whether the layer contains a non-empty layer mask subsection.
    /// </summary>
    public bool HasMaskData => _layerMaskData.RawData.Length > sizeof(uint);

    /// <summary>
    /// Gets a value indicating whether the layer contains a non-empty blending ranges subsection.
    /// </summary>
    public bool HasBlendingRangesData => _blendingRangesData.RawData.Length > sizeof(uint);

    /// <summary>
    /// Gets a value indicating whether the layer contains trailing opaque additional layer data.
    /// </summary>
    public bool HasAdditionalLayerData => _additionalLayerData.Length > 0;

    /// <summary>
    /// Gets a read-only summary of the parsed layer mask subsection.
    /// </summary>
    internal LayerMaskInfo MaskInfo => new(HasMaskData, _layerMaskData.RawData.Length);

    /// <summary>
    /// Gets a read-only summary of the parsed blending ranges subsection.
    /// </summary>
    internal LayerBlendingRangesInfo BlendingRangesInfo => new(HasBlendingRangesData, _blendingRangesData.RawData.Length);

    /// <summary>
    /// Stores the parsed per-channel metadata from the layer record.
    /// </summary>
    internal LayerChannelInfo[] ChannelInfo { get; private set; } = [];

    /// <summary>
    /// Gets the original PSD layer flags byte used when rewriting the layer record.
    /// </summary>
    internal byte RawFlags => _flags;

    /// <summary>
    /// Gets the raw layer mask subsection used by the layer record writer.
    /// </summary>
    internal LayerMaskData LayerMaskData => _layerMaskData;

    /// <summary>
    /// Gets the raw blending ranges subsection used by the layer record writer.
    /// </summary>
    internal LayerBlendingRangesData BlendingRangesData => _blendingRangesData;

    /// <summary>
    /// Gets the additional layer data bytes that follow the Pascal layer name.
    /// </summary>
    internal byte[] AdditionalLayerData => _additionalLayerData;

    /// <summary>
    /// Stores the raw layer mask subsection including its length field.
    /// </summary>
    private LayerMaskData _layerMaskData = LayerMaskData.Empty;

    /// <summary>
    /// Stores the raw blending ranges subsection including its length field.
    /// </summary>
    private LayerBlendingRangesData _blendingRangesData = LayerBlendingRangesData.Empty;

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
        return LayerRecordReader.Load(reader, isLargeDocument);
    }

    /// <summary>
    /// Writes the current layer record using PSD- or PSB-sized channel lengths.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    /// <param name="isLargeDocument">true for PSB-sized layer channel lengths; otherwise, false.</param>
    internal void Write(BigEndianWriter writer, bool isLargeDocument)
    {
        LayerRecordWriter.Write(this, writer, isLargeDocument);
    }

    /// <summary>
    /// Creates a layer instance from already-parsed PSD layer record fields without marking it as mutated.
    /// </summary>
    /// <param name="name">The parsed layer name.</param>
    /// <param name="bounds">The parsed layer bounds in PSD document coordinates.</param>
    /// <param name="isVisible">true when the layer is visible; otherwise, false.</param>
    /// <param name="opacity">The parsed opacity byte.</param>
    /// <param name="flags">The original PSD layer flags byte.</param>
    /// <param name="blendModeKey">The original 4-byte PSD blend mode key.</param>
    /// <param name="clipping">The parsed clipping value.</param>
    /// <param name="blendMode">The public blend mode mapped from <paramref name="blendModeKey"/>.</param>
    /// <param name="channelInfo">The parsed layer channel metadata.</param>
    /// <param name="layerMaskData">The raw layer mask subsection.</param>
    /// <param name="blendingRangesData">The raw blending ranges subsection.</param>
    /// <param name="additionalLayerData">The additional layer data bytes after the Pascal layer name.</param>
    /// <returns>The parsed layer domain object.</returns>
    internal static Layer CreateParsed(
        string name,
        Rectangle bounds,
        bool isVisible,
        byte opacity,
        byte flags,
        string blendModeKey,
        byte clipping,
        BlendMode blendMode,
        LayerChannelInfo[] channelInfo,
        LayerMaskData layerMaskData,
        LayerBlendingRangesData blendingRangesData,
        byte[] additionalLayerData)
    {
        return new Layer
        {
            _name = name,
            _bounds = bounds,
            _isVisible = isVisible,
            _opacity = opacity,
            _flags = flags,
            _blendModeKey = blendModeKey,
            _clipping = clipping,
            _blendMode = blendMode,
            ChannelInfo = channelInfo,
            _layerMaskData = layerMaskData,
            _blendingRangesData = blendingRangesData,
            _additionalLayerData = additionalLayerData
        };
    }
}
