using System.Text.RegularExpressions;

namespace MenuSelector.Utilities;

internal static class MenuEnvironmentObjectExtensions
{
    public static Regex PascalCaseRegex { get; } = new(@"(\B[A-Z])", RegexOptions.Compiled);

    public static string FormatEnvironmentName(string fullName) =>
        PascalCaseRegex.Replace(fullName.Replace("Environment", string.Empty), " $1");
}