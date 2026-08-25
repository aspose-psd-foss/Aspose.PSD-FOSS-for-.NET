using Aspose.PSD.FileFormats.Psd.Layers;
using System.IO;
using System.Linq;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Represents a PSD image that can be loaded, inspected, and saved without rendering.
/// </summary>
public sealed class PsdImage : Image
{
    /// <summary>
    /// Stores the underlying source or destination stream.
    /// </summary>
    private readonly Stream _stream;

    /// <summary>
    /// Indicates whether the underlying stream must remain open after disposal.
    /// </summary>
    private readonly bool _leaveOpen;

    /// <summary>
    /// Tracks whether the instance has already been disposed.
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Gets the document width in pixels.
    /// </summary>
    public override int Width => _document.Header?.Width ?? 0;

    /// <summary>
    /// Gets the document height in pixels.
    /// </summary>
    public override int Height => _document.Header?.Height ?? 0;

    /// <summary>
    /// Gets the document channel count from the PSD header.
    /// </summary>
    internal int Channels => _document.Header?.Channels ?? 0;

    /// <summary>
    /// Gets the number of bits stored per channel.
    /// </summary>
    public int BitsPerChannel => _document.Header?.BitDepth ?? 0;

    /// <summary>
    /// Gets the PSD color mode reported by the header.
    /// </summary>
    public ColorModes ColorMode
    {
        get => _document.Header?.ColorMode ?? ColorModes.Rgb;
        set => Header.SetColorMode(value);
    }

    /// <summary>
    /// Gets or sets the PSD container version.
    /// </summary>
    public int Version
    {
        get => _document.Header?.Version ?? (int)PsdVersion.Psd;
        set => Header.SetVersion(value);
    }

    /// <summary>
    /// Gets the parsed PSD/PSB header object.
    /// </summary>
    internal PsdHeader Header => _document.Header ?? throw new InvalidOperationException("PSD/PSB header is not loaded.");

    /// <summary>
    /// Gets a value indicating whether the loaded document uses the PSB large-document container.
    /// </summary>
    internal bool IsLargeDocument => _document.Header?.IsLargeDocument == true;

    /// <summary>
    /// Gets a value indicating whether the loaded document is a PSB file; this is equivalent to <see cref="IsLargeDocument"/>.
    /// </summary>
    internal bool IsPsb => IsLargeDocument;

    /// <summary>
    /// Gets the parsed layer collection.
    /// </summary>
    public Layer[] Layers
    {
        get => _document.LayerAndMaskSection.Layers.ToArray();
        set => _document = _document.WithLayerAndMaskSection(_document.LayerAndMaskSection.WithLayers(value ?? []));
    }

    /// <summary>
    /// Gets the PSD channels count.
    /// </summary>
    public int ChannelsCount => Channels;

    /// <summary>
    /// Gets the image size.
    /// </summary>
    public Size Size => new(Width, Height);

    /// <summary>
    /// Gets or sets the active layer.
    /// </summary>
    public Layer? ActiveLayer
    {
        get => Layers.FirstOrDefault();
        set => throw new NotSupportedException("Changing the active layer is not supported by this FOSS build.");
    }

    /// <summary>
    /// Gets the number of parsed layers in the document.
    /// </summary>
    internal int LayerCount => Layers.Length;

    /// <summary>
    /// Gets a value indicating whether the document contains at least one parsed layer.
    /// </summary>
    internal bool HasLayers => _document.LayerAndMaskSection.Layers.Length > 0;

    /// <summary>
    /// Gets a value indicating whether the document contains any parsed image resources.
    /// </summary>
    internal bool HasImageResources => _document.ImageResourcesSection.HasResources;

    /// <summary>
    /// Gets the number of parsed image resource blocks.
    /// </summary>
    internal int ResourceCount => _document.ImageResourcesSection.Resources.Length;

    /// <summary>
    /// Gets or sets the PSD image resources.
    /// </summary>
    public ResourceBlock[] ImageResources
    {
        get => _document.ImageResourcesSection.Resources.Select(resource => new PreservedResourceBlock(resource)).Cast<ResourceBlock>().ToArray();
        set => throw new NotSupportedException("Changing image resources is not supported by this FOSS build.");
    }

    /// <summary>
    /// Gets or sets the global layer resources.
    /// </summary>
    public LayerResource[] GlobalLayerResources
    {
        get => [];
        set => throw new NotSupportedException("Changing global layer resources is not supported by this FOSS build.");
    }

    /// <summary>
    /// Gets the global layer mask info.
    /// </summary>
    public GlobalLayerMaskInfo GlobalLayerMaskInfo => GlobalLayerMaskInfo.Empty;

    /// <summary>
    /// Gets a value indicating whether the PSD image is flattened.
    /// </summary>
    public bool IsFlatten => _document.LayerAndMaskSection.Layers.Length == 0;

    /// <summary>
    /// Gets or sets a value indicating whether first alpha channel contains the transparency data for the merged result when specifying layers data.
    /// </summary>
    public bool HasTransparencyData
    {
        get => false;
        set => throw new NotSupportedException("Changing transparency data semantics is not supported by this FOSS build.");
    }

    /// <summary>
    /// Gets a read-only summary of the parsed image resource blocks.
    /// </summary>
    internal IReadOnlyList<PsdResourceInfo> Resources => _document.ImageResourcesSection.Resources.Select(resource => resource.ToPublicInfo()).ToArray();

    /// <summary>
    /// Gets a value indicating whether the document contains Color Mode Data bytes.
    /// </summary>
    internal bool HasColorModeData => _document.ColorData.RawData.Length > 0;

    /// <summary>
    /// Gets a read-only summary of the parsed Color Mode Data section.
    /// </summary>
    internal PsdColorDataInfo ColorDataInfo => _document.ColorData.ToPublicInfo();

    /// <summary>
    /// Gets the parsed indexed palette summary, when the Color Mode Data section contains one.
    /// </summary>
    internal IndexedColorPaletteInfo? IndexedPalette => ColorDataInfo.IndexedPalette;

    /// <summary>
    /// Gets a value indicating whether the document contains merged image data payload bytes.
    /// </summary>
    internal bool HasMergedImageData => _document.ImageData.RawData.Length > 0;

    /// <summary>
    /// Gets the compression method used by the merged image data section.
    /// </summary>
    public CompressionMethod Compression => _document.ImageData.Compression;

    /// <summary>
    /// Gets a read-only summary of the parsed merged image data structure.
    /// </summary>
    internal PsdImageDataInfo ImageDataInfo => _document.ImageData.ToPublicInfo();

    /// <summary>
    /// Gets the structural kind of the merged image data payload.
    /// </summary>
    internal ImageDataKind ImageDataKind => _document.ImageData.Structure.Kind;

    /// <summary>
    /// Gets a value indicating whether ZIP prediction is used by the merged image data payload.
    /// </summary>
    internal bool UsesPrediction => _document.ImageData.Structure.UsesPrediction;

    /// <summary>
    /// Gets the parsed global angle from the image resources, when present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public int GlobalAngle { get; set; }

    /// <summary>
    /// Gets a value indicating whether an embedded ICC profile resource is present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    internal bool HasIccProfile => false;

    /// <summary>
    /// Gets the parsed untagged ICC profile flag, when the corresponding resource is present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    internal bool? IsIccProfileUntagged => null;

    /// <summary>
    /// Gets the parsed color mode data details for internal verification and tests.
    /// </summary>
    internal ColorData ParsedColorData => _document.ColorData;

    /// <summary>
    /// Gets the parsed image resources for internal verification and tests.
    /// </summary>
    internal UnknownResource[] ParsedResources => _document.ImageResourcesSection.Resources;

    /// <summary>
    /// Stores the parsed PSD/PSB document sections.
    /// </summary>
    private PsdImageDocumentState _document = PsdImageDocumentState.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="PsdImage"/> class over an internal working stream.
    /// </summary>
    /// <param name="stream">The buffered stream that stores PSD/PSB bytes for this instance.</param>
    /// <param name="leaveOpen">true to leave the stream open after disposal; otherwise, false.</param>
    private PsdImage(Stream stream, bool leaveOpen)
    {
        _stream = stream;
        _leaveOpen = leaveOpen;
    }

    /// <summary>
    /// Loads a PSD image from a file path.
    /// </summary>
    /// <param name="filePath">The path to the PSD file.</param>
    /// <returns>A loaded <see cref="PsdImage"/> instance.</returns>
    public static new PsdImage Load(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));
        if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found: {filePath}");

        using var stream = File.OpenRead(filePath);
        return Load(stream);
    }

    /// <summary>
    /// Loads a PSD image from a readable stream.
    /// </summary>
    /// <param name="stream">The input stream containing PSD data.</param>
    /// <returns>A loaded <see cref="PsdImage"/> instance.</returns>
    public static new PsdImage Load(Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        long originalPosition = 0;
        bool restorePosition = stream.CanSeek;
        if (restorePosition)
        {
            originalPosition = stream.Position;
        }

        try
        {
            var bufferedStream = new MemoryStream();
            stream.CopyTo(bufferedStream);
            bufferedStream.Position = 0;
            return Load(bufferedStream, leaveOpen: false);
        }
        finally
        {
            if (restorePosition)
            {
                stream.Position = originalPosition;
            }
        }
    }

    /// <summary>
    /// Creates a loaded image instance over the provided stream.
    /// </summary>
    /// <param name="stream">The buffered PSD/PSB stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after disposal; otherwise, false.</param>
    /// <returns>The loaded <see cref="PsdImage"/> instance.</returns>
    private static PsdImage Load(Stream stream, bool leaveOpen)
    {
        var image = new PsdImage(stream, leaveOpen);
        image._document = PsdImageLoader.Load(stream, leaveOpen);
        return image;
    }

    /// <summary>
    /// Saves the image to a file path.
    /// </summary>
    /// <param name="filePath">The destination file path.</param>
    public override void Save(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        using var stream = File.Create(filePath);
        Save(stream, leaveOpen: false);
    }

    /// <summary>
    /// Saves the image to a writable stream.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    public override void Save(Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        Save(stream, leaveOpen: true);
    }

    /// <summary>
    /// Saves the current document to a stream with configurable stream ownership.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after saving; otherwise, false.</param>
    private void Save(Stream stream, bool leaveOpen)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PsdImage));

        PsdImageWriter.Save(_document, stream, leaveOpen);
    }

    /// <summary>
    /// Releases the image and optionally the underlying stream.
    /// </summary>
    public override void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (!_leaveOpen)
        {
            _stream.Dispose();
        }
    }

}
