using System;
using System.IO;

namespace dTerm.Core
{
    public static class AppData
    {
        public static DirectoryInfo GetBaseDirectoryInfo()
        {
            var directoryInfo = new DirectoryInfo(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "dTerm"
                )
            );

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            return directoryInfo;
        }
    }
}
