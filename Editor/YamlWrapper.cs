using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
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
            else
            {
                File.Delete(GetPathToCopyYaml);
                File.Copy(GetPathToOriginalYaml, GetPathToCopyYaml);
                Debug.Log("<color=green> codemagic.yaml created at</color>\n" + Path.Combine(FindSourcePath(), "codemagic.yaml") );

            }
        }

        public static void EditCopyYaml(string textWriter, List<string> objToRemove)
        {
            CopyYAMLtoRootDir();
            
            var lines = textWriter.Split("\n");
            int countTab = 0;
            List<int> skipLines = new ();
            
            using (StreamWriter writer = new StreamWriter(GetPathToCopyYaml))
            {
                Debug.LogError(objToRemove.Count );

                for (int i = 0; i < lines.Length; i++)
                {
                    if (objToRemove.Any(remove => lines[i].Contains(remove)))
                    {
                        skipLines.Add(i);
                        for (int j = 0; j < lines[i].Length; j++)
                        {
                            if (lines[i][j] == ' ')
                            {
                                countTab++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        for (int j = i+1; j < lines.Length; j++)
                        {
                            int tab = 0;
                            for (int k = 0; k < lines[j].Length; k++)
                            {
                                if (lines[j][k] == ' ')
                                {
                                    tab++;
                                }
                            }
                            Debug.LogError(lines[j] + "  "  + countTab + "   " + tab);
                            if (tab - 3 == countTab)
                            {
                                skipLines.Add(j);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    bool notSkip = true;
                    foreach (var skip in skipLines.Where(skip => skip == i))
                    {
                        notSkip = false;
                    }
                    if(notSkip)
                        writer.WriteLine(lines[i]);
                }
                
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
                Debug.Log("<color=green>codemagic.yaml edited!</color>");
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