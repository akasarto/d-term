namespace App.Consoles.Service.PathBuilders
{
	public abstract class PathBuilderBase
	{
		internal static string NormalizeDirectory(string folderPath) => (folderPath ?? string.Empty).Trim('~', '.', '/', '\\');

		internal static string NormalizeFilename(string fileName)
		{
			fileName = (fileName ?? string.Empty).Trim('~', '.', '/', '\\');

			fileName = $"{fileName.TrimEnd(".exe".ToCharArray())}.exe";

			return fileName.Replace("/", "\\");
		}
	}
}
