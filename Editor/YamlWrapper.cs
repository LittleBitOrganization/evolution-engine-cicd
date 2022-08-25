using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace LittleBit.Modules.CICD.Editor
{
    public class YamlWrapper
    {
        [MenuItem("Tools/CICD/Create yaml")]
        private static void CopyYAMLtoRootDir()
        {
            DirectoryInfo di = new DirectoryInfo(Application.dataPath);
            string path = di.Parent.FullName;
            
            string[] picList = Directory.GetFiles(Path.Combine(FindSourcePath()),"*.yaml");

            if (!File.Exists(Path.Combine(path, "codemagic.yaml")))
            {
                File.Copy(picList[0], Path.Combine(path, "codemagic.yaml"));
                Debug.Log("<color=green> codemagic.yaml created at</color>\n" + Path.Combine(path, "codemagic.yaml") );
            }
        }

        private static string FindSourcePath([CallerFilePath] string sourceFilePath = "")
        {
            DirectoryInfo di = new DirectoryInfo(sourceFilePath);
            
            return di.Parent.FullName;
        }
    }
}
