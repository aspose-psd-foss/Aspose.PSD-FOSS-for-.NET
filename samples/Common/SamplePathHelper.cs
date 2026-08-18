namespace Aspose.PSD.FOSS.Samples.Common;

/// <summary>
/// Provides shared path resolution helpers for sample projects.
/// </summary>
internal static class SamplePathHelper
{
    /// <summary>
    /// Resolves the input PSD path from command-line arguments or the repository fixture.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The resolved PSD path, or <see langword="null"/> when no input can be found.</returns>
    internal static string? ResolveInputPath(string[] args)
    {
        if (args.Length > 0 && File.Exists(args[0]))
        {
            return Path.GetFullPath(args[0]);
        }

        string repositoryFixture = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "../../../../../src/Aspose.PSD.FOSS.Test/testdata/test.psd"));

        return File.Exists(repositoryFixture) ? repositoryFixture : null;
    }

    /// <summary>
    /// Resolves the output PSD path from command-line arguments or a default file name.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <param name="outputArgumentIndex">The zero-based argument index that may contain the output path.</param>
    /// <param name="defaultFileName">The default output file name to use when no explicit path is provided.</param>
    /// <returns>The resolved output path.</returns>
    internal static string ResolveOutputPath(string[] args, int outputArgumentIndex, string defaultFileName)
    {
        if (args.Length > outputArgumentIndex && !string.IsNullOrWhiteSpace(args[outputArgumentIndex]))
        {
            return Path.GetFullPath(args[outputArgumentIndex]);
        }

        return Path.Combine(Environment.CurrentDirectory, defaultFileName);
    }
}
