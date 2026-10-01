namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Loads parsed PSD/PSB document state from a stream.
/// </summary>
internal static class PsdImageLoader
{
    /// <summary>
    /// Loads all supported PSD/PSB sections into memory.
    /// </summary>
    /// <param name="stream">The buffered PSD/PSB stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after loading; otherwise, false.</param>
    /// <returns>The parsed document state.</returns>
    public static PsdImageDocumentState Load(Stream stream, bool leaveOpen)
    {
        var reader = new BigEndianReader(stream, leaveOpen);
        try
        {
            PsdHeader header = PsdHeader.Load(reader);
            ColorData colorData = ColorData.Load(reader, header.ColorMode);
            ImageResourcesSection imageResourcesSection = ImageResourcesSection.Load(reader);
            LayerAndMaskSection layerAndMaskSection = LayerAndMaskSection.Load(reader, header.IsLargeDocument);
            ImageData imageData = ImageData.Load(
                reader,
                header.IsLargeDocument,
                header.Height,
                header.Channels);

            return new PsdImageDocumentState(
                header,
                colorData,
                imageResourcesSection,
                layerAndMaskSection,
                imageData);
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
}
