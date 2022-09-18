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

        string buildTargetMain = project.GetUnityMainTargetGuid();
        string buildTargetUnityFramework = project.GetUnityFrameworkTargetGuid();

        List<string> frameworks = new List<string>();

        frameworks.Add("AdServices.framework");
        frameworks.Add("AdSupport.framework");
        frameworks.Add("AppTrackingTransparency.framework");
        frameworks.Add("iAd.framework");
        frameworks.Add("UnityFramework.framework");
        frameworks.Add("Security.framework");
        frameworks.Add("SystemConfiguration.framework");
        frameworks.Add("libz.dylib");

        foreach (string framework in frameworks)
        {
            Debug.Log("Adding framework: " + framework);
            project.AddFrameworkToProject(buildTargetMain, framework, true);
        }

        project.AddBuildProperty(buildTargetMain, "OTHER_LDFLAGS", "-ObjC");
        project.SetBuildProperty(buildTargetMain, "ENABLE_BITCODE", "NO");
        project.SetBuildProperty(buildTargetMain, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

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
        rootDict.SetString("NSUserTrackingUsageDescription", "For analytics");
        
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
