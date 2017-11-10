using System.Diagnostics;

namespace dTerm.Core
{
	public abstract class ProcessStartInfoBuilderBase
	{
		internal abstract ProcessStartInfo GetProcessStartInfo();

		internal static string NormalizeFolderPath(string folderPath) => (folderPath ?? string.Empty).Trim('~', '.', '/', '\\');

		internal static string NormalizeFilename(string fileName)
		{
			fileName = (fileName ?? string.Empty).Trim('~', '.', '/', '\\');

			fileName = $"{fileName.TrimEnd(".exe".ToCharArray())}.exe";

			return fileName.Replace("/", "\\");
		}
	}
}
