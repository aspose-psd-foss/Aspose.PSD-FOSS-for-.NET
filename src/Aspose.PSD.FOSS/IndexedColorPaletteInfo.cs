using System.Collections.ObjectModel;
using System.Drawing;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Provides a read-only view over an indexed-color PSD palette.
/// </summary>
public sealed class IndexedColorPaletteInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IndexedColorPaletteInfo"/> class.
    /// </summary>
    /// <param name="entries">The decoded palette entries.</param>
    public IndexedColorPaletteInfo(Color[] entries)
    {
        Entries = Array.AsReadOnly((Color[])entries.Clone());
    }

    /// <summary>
    /// Gets the decoded palette entries.
    /// </summary>
    public ReadOnlyCollection<Color> Entries { get; }
}
