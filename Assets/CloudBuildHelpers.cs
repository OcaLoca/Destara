/* ----------------------------------------------
 * 
 * 				Ariokan
 * 
 * Creation Date: 05/07/2021 17:10:20
 * 
 * Copyright � AheadGames
 * ----------------------------------------------
 */

using StarworkGC.Utils;
using System.IO;

namespace Ariokan
{
    ///<summary>
    ///A set of method invoked by Unity Cloud Build during the build process
    ///</summary>
    public static class CloudBuildHelpers
    {
        public static void PostExport(string exportPath)
        {
            FileAttributes attr = File.GetAttributes(exportPath);
            string directory;
            if (attr.HasFlag(FileAttributes.Directory))
            {
                directory = exportPath;
            }
            else
            {
                directory = Path.GetDirectoryName(exportPath);
            }
           // CopyBasePreferences(directory);

        }
       // static void CopyBasePreferences(string buildPath)
       // {
       //     CopyDirectory(FilePaths.ADDITIONAL_BUILD_FILES_FOLDER, buildPath, true);
       // }

        static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}