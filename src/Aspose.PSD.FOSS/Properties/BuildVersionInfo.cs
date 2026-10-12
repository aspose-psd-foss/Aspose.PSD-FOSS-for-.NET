using System;
using System.Runtime.CompilerServices;
using Aspose.PSD;

[assembly: System.Reflection.AssemblyTitle(AssemblyConstants.Title)]
[assembly: System.Reflection.AssemblyDescription(AssemblyConstants.Product)]
[assembly: System.Reflection.AssemblyConfiguration("")]
[assembly: System.Reflection.AssemblyCompany("Aspose Pty Ltd.")]
[assembly: System.Reflection.AssemblyProduct(AssemblyConstants.Product)]
[assembly: System.Reflection.AssemblyCopyright("Copyright © 2001-2026 Aspose Pty Ltd.")]
[assembly: System.Reflection.AssemblyTrademark(AssemblyConstants.Product)]
[assembly: System.Reflection.AssemblyCulture("")]
[assembly: System.Reflection.AssemblyVersion(AssemblyConstants.AssemblyVersion)]
[assembly: System.Reflection.AssemblyInformationalVersion(AssemblyConstants.InformationalVersion)]

[assembly: System.Reflection.Obfuscation(Feature = "ignore InternalsVisibleToAttribute", Exclude = false)]
[assembly: System.Reflection.AssemblyFileVersion(AssemblyConstants.FileVersion)]
[assembly: System.Security.AllowPartiallyTrustedCallers]

#if !DEBUG
[assembly: System.Reflection.AssemblyKeyFile("..\\..\\publish\\Aspose.snk")]
#endif

#if DEBUG
[assembly: InternalsVisibleTo("Aspose.PSD.FOSS.Test")]
#else
[assembly: InternalsVisibleTo("Aspose.PSD.FOSS.Test, PublicKey=" + Aspose.FOSS.Security.PublicKey.Value)]
#endif

namespace Aspose.PSD
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Contains the current build version information.
    /// </summary>
    public static class BuildVersionInfo
    {
        /// <summary>
        /// The assembly version.
        /// </summary>
        public static readonly string AssemblyVersion = AssemblyConstants.AssemblyVersion;

        /// <summary>
        /// The file version.
        /// </summary>
        public static readonly string FileVersion = AssemblyConstants.FileVersion;

        /// <summary>
        /// The product title.
        /// </summary>
        public static readonly string Product = AssemblyConstants.Product;

        /// <summary>
        /// The major product version.
        /// </summary>
        public static readonly int ProductMajor = AssemblyConstants.ProductMajor;

        /// <summary>
        /// The minor product version.
        /// </summary>
        public static readonly int ProductMinor = AssemblyConstants.ProductMinor;        

        /// <summary>
        /// The product release date.
        /// </summary>
        public static readonly DateTime ReleaseDate = DateTime.Parse(AssemblyConstants.ReleaseDate, CultureInfo.CreateSpecificCulture("en-US"));
    }
}
