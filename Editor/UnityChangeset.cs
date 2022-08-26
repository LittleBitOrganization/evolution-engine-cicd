using System.IO;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;

namespace LittleBit.Modules.CICD.Editor
{
    public class UnityChangeset
    {
        private const string ProjectVersionFileName = "ProjectVersion.txt";
        private static string PathToVersion => Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "ProjectSettings");

        private static string FullPathToFile => Path.Combine(PathToVersion, ProjectVersionFileName);

        public static string GetUnityChangeset()
        {
            using (StreamReader reader = File.OpenText(FullPathToFile))
            {
                var deserializer = new DeserializerBuilder()
                    .Build();
                var yamlObject = deserializer.Deserialize<YamlVersionModel>(new StreamReader(FullPathToFile));
                
                return yamlObject.m_EditorVersionWithRevision.Split(" ").First(x => x.StartsWith("(") && x.EndsWith(")")).Trim('(', ')');
            }
        }
    }
}