using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using UnityEngine;
using UnityEditor;

namespace AutoPackage
{
    public static class Packages
    {
        public static async Task ReplacePackagesFromGist(string id, string user)
        {
            var url = Packages.GetGistUrl(id,user);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        public static string GetGistUrl(string id, string user) =>
          $"https://gist.github.com/{user}/{id}/raw";

        public static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        
        public static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing,contents);
            UnityEditor.PackageManager.Client.Resolve();
        }
        
        public static void InstallUnityPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }
        
        [MenuItem("Tools/Setup/Packages/2D Sprite")]
        static void Add2DSprite() => Packages.InstallUnityPackage("2d.sprite");
        
        [MenuItem("Tools/Setup/Packages/2D Tilemap")]
        static void Add2DTilemap() => Packages.InstallUnityPackage("2d.tilemap");
        
        [MenuItem("Tools/Setup/Packages/WebGL Publisher")]
        static void AddWebGLPublisher() => Packages.InstallUnityPackage("connect.share");
    }
}
