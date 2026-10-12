namespace Aspose.PSD
{
    /// <summary>
    /// The assembly constants
    /// </summary>
    internal class AssemblyConstants
    {
#if NETSTANDARD
        /// <summary>
        /// The assembly title
        /// </summary>
        internal const string Title = ".NET Standard 2.1";
#elif NET10_0
        /// <summary>
        /// The assembly title
        /// </summary>
        internal const string Title = ".NET 10.0";
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
        internal const string Description = "Aspose.PSD FOSS for .NET provides open-source code for PSD/PSB File Processing";

        /// <summary>
        /// The file version
        /// </summary>
        internal const string FileVersion = AssemblyVersion;

        /// <summary>
        /// The release date in format 'YYYY.MM.DD'
        /// </summary>
        internal const string ReleaseDate = BaseAssemblyConstants.ReleaseDate;

        /// <summary>
        /// The assembly version
        /// </summary>
        internal const string AssemblyVersion = BaseAssemblyConstants.AssemblyVersion;

        /// <summary>
        /// The product major
        /// </summary>
        internal const int ProductMajor = BaseAssemblyConstants.ProductMajor;

        /// <summary>
        /// The product minor
        /// </summary>
        internal const int ProductMinor = BaseAssemblyConstants.ProductMinor;

        /// <summary>
        /// The informational version
        /// </summary>
        internal const string InformationalVersion = AssemblyVersion;
    }
}
