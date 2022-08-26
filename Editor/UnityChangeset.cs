using System.IO;
using System.Linq;
using UnityEngine;

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
                string line = reader.ReadToEnd();

                line = line.Split("\n").First(x => x.StartsWith("m_EditorVersionWithRevision:"))
                    .Split(" ").First(x => x.StartsWith("(") && x.EndsWith(")")).Trim('(', ')');

                return line;
            }
        }
    }
}