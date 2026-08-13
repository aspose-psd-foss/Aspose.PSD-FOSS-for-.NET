namespace Aspose.PSD.FOSS;

public class PsdLoadException : Exception
{
    public PsdLoadException(string message) : base(message) { }
}

public class PsdSaveException : Exception
{
    public PsdSaveException(string message) : base(message) { }
}
