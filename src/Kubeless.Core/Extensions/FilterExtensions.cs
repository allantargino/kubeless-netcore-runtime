using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kubeless.Core.Filters
{
    public static class FiltersExtensions
    {
        public static IEnumerable<string> ApplyFilterOnDllVersion(this IEnumerable<string> dllPaths)
        {
            var files = from dll in dllPaths select new FileInfo(dll);
            var pickedFiles = new List<string>();
            var pickedPaths = new List<string>();
            foreach (var file in files)
            {
                if (!pickedFiles.Contains(file.Name))
                {
                    pickedFiles.Add(file.Name);
                    pickedPaths.Add(file.FullName);
                }
            }

            return pickedPaths;
        }
    }
}
