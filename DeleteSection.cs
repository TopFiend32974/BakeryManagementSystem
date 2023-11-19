using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delete_Push_Pull
{
    internal class DeleteSection
    {
        static bool IsFolderNotEmpty(string folderPath)
        {
            // Check if the folder exists
            if (Directory.Exists(folderPath))
            {
                // Check if there are any files in the folder
                return Directory.EnumerateFileSystemEntries(folderPath).Any();
            }
            else
            {
                // The folder does not exist
                return false;
            }
        }

        public static void DeleteFiles(string folderPath)
        {
            // Check if the folder exists
            if (Directory.Exists(folderPath))
            {
                // Check if there are any files in the folder
                if (IsFolderNotEmpty(folderPath))
                {
                    // Delete all files in the folder
                    foreach (string file in Directory.EnumerateFiles(folderPath))
                    {
                        File.Delete(file);
                    }
                }
            }
        }   
    }
}
