namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Defines the base class for PSD layer mask data.
/// </summary>
public abstract class LayerMaskData
{
    /// <summary>
    /// Stores the layer mask image data.
    /// </summary>
    private byte[] _imageData = [];

    /// <summary>
    /// Stores the layer mask rectangle.
    /// </summary>
    private Rectangle _maskRectangle;

    /// <summary>
    /// Gets or sets the bottom layer mask position.
    /// </summary>
    public int Bottom
    {
        get => _maskRectangle.Bottom;
        set => _maskRectangle.Bottom = value;
    }

    /// <summary>
    /// Gets the size of the layer mask data.
    /// </summary>
    public int DataSize => _imageData.Length;

    /// <summary>
    /// Gets or sets the default layer mask color.
    /// </summary>
    public byte DefaultColor { get; set; }

    /// <summary>
    /// Gets or sets the layer mask flags.
    /// </summary>
    public LayerMaskFlags Flags { get; set; }

    /// <summary>
    /// Gets or sets the layer mask image data.
    /// </summary>
    public byte[] ImageData
    {
        get => _imageData;
        set => _imageData = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the left layer mask position.
    /// </summary>
    public int Left
    {
        get => _maskRectangle.Left;
        set => _maskRectangle.Left = value;
    }

    /// <summary>
    /// Gets or sets the mask rectangle.
    /// </summary>
    public Rectangle MaskRectangle
    {
        get => _maskRectangle;
        set => _maskRectangle = value;
    }

    /// <summary>
    /// Gets or sets the right layer mask position.
    /// </summary>
    public int Right
    {
        get => _maskRectangle.Right;
        set => _maskRectangle.Right = value;
    }

    /// <summary>
    /// Gets or sets the top layer mask position.
    /// </summary>
    public int Top
    {
        get => _maskRectangle.Top;
        set => _maskRectangle.Top = value;
    }
}
