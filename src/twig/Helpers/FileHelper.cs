﻿namespace twig
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public static class FileHelper
    {
        public static long GetDirectorySize(string path, bool subfolder, string ext = ".")
        {
            IEnumerable<string> dir = null;
            var option = SearchOption.TopDirectoryOnly;
            if (subfolder)
            {
                option = SearchOption.AllDirectories;
            }

            if (ext == ".zs")
            {
                dir = Directory.GetFiles(path, "*.zs*", option);
            }
            else
            {
                dir = Directory.GetFiles(path, "*.*", option).Where(path => !path.EndsWith(".zs"));
            }

            long total = 0;

            foreach (var file in dir)
            {
                FileInfo info = new FileInfo(file);
                total += info.Length;
            }

            return total;
        }
        public static long GetTotalSize(string path, bool subfolder)
        {
            var compressedSize = GetDirectorySize(path, subfolder, ".zs");
            var uncompressedSize =GetDirectorySize(path, subfolder);
            var total = compressedSize + uncompressedSize;
            return total;
        }

        public static async Task<string> WriteFileAsync(byte[] data, string path, string output = "", string extension = "")
        {
            var fileName = Path.GetFileName(path) + extension;
            var currentDirectory = Path.GetDirectoryName(path);
            var directory = currentDirectory;
            if (!String.IsNullOrEmpty(output))
            {
                var outputPath = Directory.CreateDirectory(output).ToString();
                directory = outputPath;
            }

            var writePath = Path.Combine(directory, fileName);

            await using (FileStream fstream = new FileStream(writePath, FileMode.Create))
            {
                await fstream.WriteAsync(data, 0, data.Length);
            }

            return writePath;
        }
    }
}
