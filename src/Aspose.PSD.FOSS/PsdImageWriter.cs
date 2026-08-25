namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Writes parsed PSD/PSB document state to a stream.
/// </summary>
internal static class PsdImageWriter
{
    /// <summary>
    /// Saves a parsed document state to a writable stream.
    /// </summary>
    /// <param name="document">The parsed document state to write.</param>
    /// <param name="stream">The destination stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after saving; otherwise, false.</param>
    public static void Save(PsdImageDocumentState document, Stream stream, bool leaveOpen)
    {
        var writer = new BigEndianWriter(stream, leaveOpen);
        try
        {
            writer.Write((uint)PsdHeader.PsdSignature);
            document.Header?.Save(writer);
            document.ColorData.Save(writer);
            document.ImageResourcesSection.Save(writer);
            document.LayerAndMaskSection.Save(writer, document.Header?.IsLargeDocument == true);
            document.ImageData.Save(writer);
        }
        finally
        {
            writer.Dispose();
        }
    }
}
