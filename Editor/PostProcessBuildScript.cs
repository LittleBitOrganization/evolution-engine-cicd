using System.IO;
using UnityEditor;
#if UNITY_IOS 
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;
public class PostProcessBuildScript : ScriptableObject
{
    public DefaultAsset entitlementsFile;
    
    public static void ChangesToXcode(string pathToBuiltProject)
    {
#if UNITY_IOS 
        // Get project PBX file
        var projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        var proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));
        var target = proj.GetUnityMainTargetGuid();
        // Create an instance of this ScriptableObject so we can point to its entitlements file
        var dummy = CreateInstance<PostProcessBuildScript>();
        var entitlementsFile = dummy.entitlementsFile;
        DestroyImmediate(dummy);
        if (entitlementsFile != null)
        {
            // Copy the entitlement file to the xcode project
            var entitlementPath = AssetDatabase.GetAssetPath(entitlementsFile);
            var entitlementFileName = Path.GetFileName(entitlementPath);
            var unityTarget = "Unity-iPhone";
            var relativeDestination = unityTarget + "/" + entitlementFileName;
            FileUtil.CopyFileOrDirectory(entitlementPath, pathToBuiltProject + "/" + relativeDestination);
            // Add the pbx configs to include the entitlements files on the project
            proj.AddFile(relativeDestination, entitlementFileName);
            proj.AddBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", relativeDestination);
            // Add push notifications as a capability on the target
            proj.AddBuildProperty(target, "SystemCapabilities", "{com.apple.Push = {enabled = 1;};}");
        }
        // Save the changed configs to the file
        File.WriteAllText(projPath, proj.WriteToString());
#endif
    }
}