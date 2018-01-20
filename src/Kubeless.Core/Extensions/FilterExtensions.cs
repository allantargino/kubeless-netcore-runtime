namespace Kubeless.Core.Filters
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class FiltersExtensions
    {
        public static IEnumerable<string> ApplyFilterOnDllVersion(this IEnumerable<string> dllPaths)
        {
            IEnumerable<FileInfo> files = from dll in dllPaths select new FileInfo(dll);
            IList<string> pickedFiles = new List<string>();
            IList<string> pickedPaths = new List<string>();
            
            foreach (FileInfo file in files)
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
