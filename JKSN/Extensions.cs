using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSN
{
    internal static class Extensions
    {
        public static string PrependPath(this string path, string prepend)
        {
            if (string.IsNullOrEmpty(path))
                return prepend;
            if (string.IsNullOrEmpty(prepend))
                return path;
            if (path.StartsWith(prepend, StringComparison.OrdinalIgnoreCase))
                return path;
            
            var drive = "/";
            if (OperatingSystem.IsWindows())
            {
                drive = path.Substring(0, 3);
                path = path.Substring(2, path.Length - 2);
            }
            return Path.Combine(drive, $"{prepend}{path}");
            
        } 
    }
}
