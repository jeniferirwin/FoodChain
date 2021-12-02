using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;

namespace AutoPackage
{
    public static class Folders
    {
        public static void CreateDirectories(string root, params string[] dir)
        {
            foreach (var newDirectory in dir)
            {
                CreateDirectory(Combine(dataPath, root, newDirectory));
            }
        }
    }
}
