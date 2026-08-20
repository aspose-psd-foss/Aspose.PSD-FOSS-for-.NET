using Aspose.PSD.FileFormats.Psd.Layers;
using System.IO;

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
    public override int Width => _header?.Width ?? 0;

    /// <summary>
    /// Gets the document height in pixels.
    /// </summary>
    public override int Height => _header?.Height ?? 0;

    /// <summary>
    /// Gets the document channel count from the PSD header.
    /// </summary>
    public int Channels => _header?.Channels ?? 0;

    /// <summary>
    /// Gets the number of bits stored per channel.
    /// </summary>
    public int BitsPerChannel => _header?.BitDepth ?? 0;

    /// <summary>
    /// Gets the PSD color mode reported by the header.
    /// </summary>
    public ColorModes ColorMode
    {
        get => _header?.ColorMode ?? ColorModes.Rgb;
        set => Header.SetColorMode(value);
    }

    /// <summary>
    /// Gets the raw PSD container version from the header: 1 for PSD and 2 for PSB.
    /// </summary>
    public int Version => _header?.Version ?? 1;

    /// <summary>
    /// Gets the parsed PSD/PSB header object.
    /// </summary>
    public PsdHeader Header => _header ?? throw new InvalidOperationException("PSD/PSB header is not loaded.");

    /// <summary>
    /// Gets a value indicating whether the loaded document uses the PSB large-document container.
    /// </summary>
    public bool IsLargeDocument => _header?.IsLargeDocument == true;

    /// <summary>
    /// Gets a value indicating whether the loaded document is a PSB file; this is equivalent to <see cref="IsLargeDocument"/>.
    /// </summary>
    public bool IsPsb => IsLargeDocument;

    /// <summary>
    /// Gets the parsed layer collection.
    /// </summary>
    public Layer[] Layers
    {
        get => _layerAndMaskSection.Layers.ToArray();
        set => _layerAndMaskSection = _layerAndMaskSection.WithLayers(value ?? []);
    }

    /// <summary>
    /// Gets the PSD channels count.
    /// </summary>
    public int ChannelsCount => Channels;

    /// <summary>
    /// Gets the number of parsed layers in the document.
    /// </summary>
    public int LayerCount => Layers.Length;

    /// <summary>
    /// Gets a value indicating whether the document contains at least one parsed layer.
    /// </summary>
    public bool HasLayers => _layerAndMaskSection.Layers.Length > 0;

    /// <summary>
    /// Gets a value indicating whether the document contains any parsed image resources.
    /// </summary>
    public bool HasImageResources => _imageResourcesSection.HasResources;

    /// <summary>
    /// Gets the number of parsed image resource blocks.
    /// </summary>
    public int ResourceCount => _imageResourcesSection.Resources.Length;

    /// <summary>
    /// Gets a read-only summary of the parsed image resource blocks.
    /// </summary>
    public IReadOnlyList<PsdResourceInfo> Resources => _imageResourcesSection.Resources.Select(resource => resource.ToPublicInfo()).ToArray();

    /// <summary>
    /// Gets a value indicating whether the document contains Color Mode Data bytes.
    /// </summary>
    public bool HasColorModeData => _colorData.RawData.Length > 0;

    /// <summary>
    /// Gets a read-only summary of the parsed Color Mode Data section.
    /// </summary>
    public PsdColorDataInfo ColorDataInfo => _colorData.ToPublicInfo();

    /// <summary>
    /// Gets the parsed indexed palette summary, when the Color Mode Data section contains one.
    /// </summary>
    public IndexedColorPaletteInfo? IndexedPalette => ColorDataInfo.IndexedPalette;

    /// <summary>
    /// Gets a value indicating whether the document contains merged image data payload bytes.
    /// </summary>
    public bool HasMergedImageData => _imageData.RawData.Length > 0;

    /// <summary>
    /// Gets the compression method used by the merged image data section.
    /// </summary>
    public CompressionMethod Compression => _imageData.Compression;

    /// <summary>
    /// Gets a read-only summary of the parsed merged image data structure.
    /// </summary>
    public PsdImageDataInfo ImageDataInfo => _imageData.ToPublicInfo();

    /// <summary>
    /// Gets the structural kind of the merged image data payload.
    /// </summary>
    public ImageDataKind ImageDataKind => _imageData.Structure.Kind;

    /// <summary>
    /// Gets a value indicating whether ZIP prediction is used by the merged image data payload.
    /// </summary>
    public bool UsesPrediction => _imageData.Structure.UsesPrediction;

    /// <summary>
    /// Gets the parsed global angle from the image resources, when present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public int GlobalAngle { get; set; }

    /// <summary>
    /// Gets a value indicating whether an embedded ICC profile resource is present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public bool HasIccProfile => false;

    /// <summary>
    /// Gets the parsed untagged ICC profile flag, when the corresponding resource is present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public bool? IsIccProfileUntagged => null;

    /// <summary>
    /// Gets the parsed color mode data details for internal verification and tests.
    /// </summary>
    internal ColorData ParsedColorData => _colorData;

    /// <summary>
    /// Gets the parsed image resources for internal verification and tests.
    /// </summary>
    internal UnknownResource[] ParsedResources => _imageResourcesSection.Resources;

    /// <summary>
    /// Stores the parsed PSD header.
    /// </summary>
    private PsdHeader? _header;

    /// <summary>
    /// Stores the parsed Color Mode Data section.
    /// </summary>
    private ColorData _colorData = ColorData.Empty;

    /// <summary>
    /// Stores the parsed and raw-preserved Image Resources section.
    /// </summary>
    private ImageResourcesSection _imageResourcesSection = ImageResourcesSection.Empty;

    /// <summary>
    /// Stores the parsed and raw-preserved Layer and Mask Information section.
    /// </summary>
    private LayerAndMaskSection _layerAndMaskSection = LayerAndMaskSection.Empty;

    /// <summary>
    /// Stores the parsed Image Data section.
    /// </summary>
    private ImageData _imageData = new(CompressionMethod.Raw, []);

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
        image.LoadInternal();
        return image;
    }

    /// <summary>
    /// Loads all supported PSD/PSB sections into memory.
    /// </summary>
    private void LoadInternal()
    {
        if (_header != null) return;

        var reader = new BigEndianReader(_stream, _leaveOpen);
        try
        {
            _header = PsdHeader.Load(reader);

            LoadColorData(reader);
            LoadResources(reader);
            LoadLayerAndMaskInfo(reader);
            LoadImageData(reader);
        }
        catch (PsdLoadException)
        {
            throw;
        }
        catch (EndOfStreamException exception)
        {
            throw new PsdLoadException("Unexpected end of PSD/PSB data while reading the file structure.", exception);
        }
        catch (IOException exception)
        {
            throw new PsdLoadException("Failed to read PSD/PSB data from the source stream.", exception);
        }
        finally
        {
            reader.Dispose();
        }
    }

    /// <summary>
    /// Loads the raw Color Mode Data section.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    private void LoadColorData(BigEndianReader reader)
    {
        _colorData = ColorData.Load(reader, ColorMode);
    }

    /// <summary>
    /// Loads the Image Resources section and preserves its raw payload for round-trip saves.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    private void LoadResources(BigEndianReader reader)
    {
        _imageResourcesSection = ImageResourcesSection.Load(reader);
    }

    /// <summary>
    /// Loads the Layer and Mask Information section and splits it into parsed and raw-preserved parts.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    private void LoadLayerAndMaskInfo(BigEndianReader reader)
    {
        _layerAndMaskSection = LayerAndMaskSection.Load(reader, _header?.IsLargeDocument == true);
    }

    /// <summary>
    /// Loads the final Image Data section as raw bytes after reading the compression field.
    /// </summary>
    /// <param name="reader">The reader positioned at the image data compression field.</param>
    private void LoadImageData(BigEndianReader reader)
    {
        _imageData = ImageData.Load(
            reader,
            _header?.IsLargeDocument == true,
            Height,
            Channels);
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

        var writer = new BigEndianWriter(stream, leaveOpen);
        try
        {
            writer.Write((uint)PsdHeader.PsdSignature);
            WriteHeader(writer);
            WriteColorData(writer);
            WriteResources(writer);
            WriteLayerAndMaskInfo(writer);
            WriteImageData(writer);
        }
        finally
        {
            writer.Dispose();
        }
    }

    /// <summary>
    /// Writes the fixed PSD/PSB header block.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteHeader(BigEndianWriter writer)
    {
        _header?.Save(writer);
    }

    /// <summary>
    /// Writes the Color Mode Data section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteColorData(BigEndianWriter writer)
    {
        _colorData.Save(writer);
    }

    /// <summary>
    /// Writes the Image Resources section using either raw-preserved data or parsed resource blocks.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteResources(BigEndianWriter writer)
    {
        _imageResourcesSection.Save(writer);
    }

    /// <summary>
    /// Writes the Layer and Mask Information section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteLayerAndMaskInfo(BigEndianWriter writer)
    {
        _layerAndMaskSection.Save(writer, _header?.IsLargeDocument == true);
    }

    /// <summary>
    /// Writes the final Image Data section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteImageData(BigEndianWriter writer)
    {
        _imageData.Save(writer);
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
