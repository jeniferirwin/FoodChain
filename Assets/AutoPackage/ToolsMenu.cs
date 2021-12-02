using static UnityEditor.AssetDatabase;
using UnityEditor;

namespace AutoPackage
{
    public static class ToolsMenu
    {
        private static string id = "bbb688a7849318d25a77363a8596e456";
        private static string user = "jeniferirwin";

        [MenuItem("Tools/Setup/Create Default Folders")]
        static void CreateDefaultFolders()
        {
            Folders.CreateDirectories("_Project", "Scripts", "Prefabs", "Materials", "Audio", "Other");
            Refresh();
        }

        [MenuItem("Tools/Setup/Load New Manifest")]
        static async void LoadNewManifest() => await Packages.ReplacePackagesFromGist(id,user);
    }
}