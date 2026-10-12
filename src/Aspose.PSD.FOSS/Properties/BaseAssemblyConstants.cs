namespace Aspose.PSD
{
    /// <summary>
    /// The assembly constants
    /// </summary>
    internal class BaseAssemblyConstants
    {
#if NETSTANDARD
        /// <summary>
        /// The assembly title
        /// </summary>
        internal const string ProductFor = ".NET Standard 2.0";
#elif NET10_0
        /// <summary>
        /// The assembly title
        /// </summary>
        internal const string ProductFor = ".NET 10.0";
#endif

        /// <summary>
        /// The assembly product
        /// </summary>        
        internal const string Product = "Aspose.PSD FOSS for " + Platform;

        /// <summary>
        /// The assembly family
        /// </summary>
        internal const string Family = "Aspose.PSD FOSS";

        /// <summary>
        /// The assembly platform
        /// </summary>        
        internal const string Platform = ".NET";

        /// <summary>
        /// The description
        /// </summary>
        internal const string Description = "Library for PSD/PSB file formats processing";

        /// <summary>
        /// The file version
        /// </summary>
        internal const string FileVersion = AssemblyVersion;

        /// <summary>
        /// The release date in format 'YYYY.MM.DD'
        /// </summary>
        internal const string ReleaseDate = "2026.10.01";

        /// <summary>
        /// The assembly version
        /// </summary>
        internal const string AssemblyVersion = "26.10";

        /// <summary>
        /// The product major
        /// </summary>
        internal const int ProductMajor = 26;

        /// <summary>
        /// The product minor
        /// </summary>
        internal const int ProductMinor = 10;

        /// <summary>
        /// The informational version
        /// </summary>
        internal const string InformationalVersion = AssemblyVersion;

        internal const string Copyright = "Copyright © 2001-2026 Aspose Pty Ltd.";

        internal const string Owner = "Aspose Pty Ltd.";
    }
}
