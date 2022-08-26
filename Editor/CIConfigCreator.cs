using System.IO;
using UnityEditor;
using UnityEngine;

namespace LittleBit.Modules.CICD.Editor
{
    public class CIConfigCreator
    {
        private const string ConfigsFolderPath = "Assets/Resources/Configs";
        private const string ResourcesPath = "Assets/Resources";
        private const string ConfigFolderName = "Configs";
        private const string Extension = ".asset";
        private const string CicdConfigName = "CICDConfig";
        [MenuItem("Tools/CICD/Create config")]
        private static void CreateConfig()
        {
            var instance = ScriptableObject.CreateInstance<CICDConfig>();
            
            CheckOrCreateDirectory();
            
            AssetDatabase.CreateAsset(instance, GetFilePath());

            AssetDatabase.SaveAssets();
        }
        
        private static void CheckOrCreateDirectory()
        {
            if (AssetDatabase.IsValidFolder(ResourcesPath) == false)
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
                AssetDatabase.CreateFolder(ResourcesPath, ConfigFolderName);
                return;
            }

            if (AssetDatabase.IsValidFolder(ConfigsFolderPath) == false)
                AssetDatabase.CreateFolder(ResourcesPath, ConfigFolderName);
        }

        private static string GetFilePath() => Path.Combine(ConfigsFolderPath, (CicdConfigName + Extension));
    }
}