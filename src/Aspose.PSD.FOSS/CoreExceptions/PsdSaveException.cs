namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Exception that is thrown when an error occurs while saving a PSD file.
/// </summary>
public class PsdSaveException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdSaveException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public PsdSaveException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PsdSaveException"/> class with a specified error message and inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public PsdSaveException(string message, Exception innerException) : base(message, innerException) { }
}
