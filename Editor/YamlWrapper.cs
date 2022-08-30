using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LittleBit.Modules.CICD.Editor
{
    public class YamlWrapper
    {
        private const string YamlName = "codemagic.yaml";
        private const string ProjectSettingsFileName = "ProjectSettings.asset";
        private static string PathToVersion => Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "ProjectSettings");
        private static string FullPathToProjectSettings => Path.Combine(PathToVersion, ProjectSettingsFileName);
        private static string GetPathToCopyYaml => Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, YamlName);
        private static string GetPathToOriginalYaml => Directory.GetFiles(Path.Combine(FindSourcePath()),"*.yaml")[0];

        public static void CopyYAMLtoRootDir()
        {
            if (!File.Exists(GetPathToOriginalYaml))
            {
                File.Copy(GetPathToOriginalYaml, GetPathToCopyYaml);
                Debug.Log("<color=green> codemagic.yaml created at</color>\n" + Path.Combine(FindSourcePath(), "codemagic.yaml") );
            }
        }

        public static void EditCopyYaml(string textWriter)
        {
            CopyYAMLtoRootDir();
            using (StreamWriter writer = new StreamWriter(GetPathToCopyYaml))  
            {  
                writer.Write(textWriter); 
                Debug.Log("<color=green> codemagic.yaml edited!</color>");
            }
        }
        
        public static TextReader GetTextYaml()
        {
            return new StreamReader(GetPathToOriginalYaml);
        }

        public static TextReader GetProjectSettingYaml()
        {
            using (StreamWriter writer = new StreamWriter(FullPathToProjectSettings))  
            {  
                 
                Debug.Log("<color=green> codemagic.yaml edited!</color>");
            }
            
            return new StreamReader(FullPathToProjectSettings);
        }
        
        private static string FindSourcePath([CallerFilePath] string sourceFilePath = "")
        {
            DirectoryInfo di = new DirectoryInfo(sourceFilePath);
            
            return di.Parent.FullName;
        }
    }
}