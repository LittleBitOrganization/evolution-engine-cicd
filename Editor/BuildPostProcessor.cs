using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS 
using UnityEditor.iOS.Xcode;
#endif

using System.IO;
using System.Collections.Generic;

public class BuildPostProcessor : MonoBehaviour
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            BuildiOS(path: path);
        }
        else if (buildTarget == BuildTarget.Android)
        {
            BuildAndroid(path: path);
        }
    }

    private static void BuildAndroid(string path = "")
    {
        
    }

    private static void BuildiOS(string path = "")
    {
#if UNITY_IOS

        string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject project = new PBXProject();
        project.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
        string buildTarget = project.GetUnityFrameworkTargetGuid();
#else
    string buildTarget = project.TargetGuidByName("Unity-iPhone");
#endif

        List<string> frameworks = new List<string>();

        frameworks.Add("AdServices.framework");
        frameworks.Add("AdSupport.framework");
        frameworks.Add("AppTrackingTransparency.framework");
        frameworks.Add("iAd.framework");
        frameworks.Add("StoreKit.framework");

        foreach (string framework in frameworks)
        {
            Debug.Log("Adding framework: " + framework);
            project.AddFrameworkToProject(buildTarget, framework, true);
        }

        Debug.Log("Adding -ObjC flag to other linker flags (OTHER_LDFLAGS)");
        project.AddBuildProperty(buildTarget, "OTHER_LDFLAGS", "-ObjC");

        File.WriteAllText(projectPath, project.WriteToString());
#endif  
    }

#if UNITY_IOS
    [PostProcessBuild(1)]
    public static void EditPlist(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
            return;


        string plistPath = path + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        PlistElementDict rootDict = plist.root;

        // Add ITSAppUsesNonExemptEncryption to Info.plist
        rootDict.SetString("ITSAppUsesNonExemptEncryption", "false");


        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
