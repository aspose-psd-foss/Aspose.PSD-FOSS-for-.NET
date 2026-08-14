namespace Aspose.PSD.FOSS;

/// <summary>
/// Exception that is thrown when an error occurs while loading a PSD file.
/// </summary>
public class PsdLoadException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdLoadException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public PsdLoadException(string message) : base(message) { }
}

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
}
