using System.Linq;
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildAndroid();
        BuildIos();
        BuildWindows();
    }

    [MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
        //
        // string cacheServerIP = Environment.GetEnvironmentVariable("CACHE_SERVER_IP");
        // string cacheServerPort = Environment.GetEnvironmentVariable("CACHE_SERVER_PORT");
        //
        // // Enable Cache Server
        // if(cacheServerIP.Length > 0 && cacheServerPort.Length > 0)
        // {
        //     EditorPrefs.SetBool("CacheServerEnabled", true);
        //     EditorPrefs.SetString("CacheServerIPAddress", cacheServerIP + ":" + cacheServerPort);
        // }

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        // Set bundle version
        var versionIsSet = int.TryParse(Environment.GetEnvironmentVariable("NEW_BUILD_NUMBER"), out int parsedVersion);
        int version = 0;
        if(versionIsSet)
        {
            version = parsedVersion;
        }
        else
        {
            int projectBuildNumber = int.Parse(Environment.GetEnvironmentVariable("PROJECT_BUILD_NUMBER"));
            version = projectBuildNumber + 1;
        }

        Debug.Log($"Bundle version code set to {version}");
        PlayerSettings.Android.bundleVersionCode = version;

        // Set keystore name
        string keystoreName = Environment.GetEnvironmentVariable("CM_KEYSTORE_PATH");
        if (!String.IsNullOrEmpty(keystoreName))
        {
            Debug.Log($"Setting path to keystore: {keystoreName}");
            PlayerSettings.Android.keystoreName = keystoreName;
        }
        else
        {
            Debug.Log("Keystore name not provided");
        }

        // Set keystore password
        string keystorePass = Environment.GetEnvironmentVariable("CM_KEYSTORE_PASSWORD");
        if (!String.IsNullOrEmpty(keystorePass))
        {
            Debug.Log($"Setting keystore password: {keystorePass}");
            PlayerSettings.Android.keystorePass = keystorePass;
        }
        else
        {
            Debug.Log("Keystore password not provided");
        }

        // Set keystore alias name
        string keyaliasName = Environment.GetEnvironmentVariable("CM_KEY_ALIAS");
        if (!String.IsNullOrEmpty(keyaliasName))
        {
            Debug.Log($"Setting keystore alias: {keyaliasName}");
            PlayerSettings.Android.keyaliasName = keyaliasName;
        }
        else
        {
            Debug.Log("Keystore alias not provided");
        }

        // Set keystore password
        string keyaliasPass = Environment.GetEnvironmentVariable("CM_KEY_PASSWORD");
        if (!String.IsNullOrEmpty(keyaliasPass))
        {
            Debug.Log($"Setting keystore alias password: {keyaliasPass}");
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
        }
        else
        {
            Debug.Log("Keystore alias password not provided");
        }

        EditorUserBuildSettings.buildAppBundle = true;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "android/" + Application.productName + ".aab";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building Android AAB");
        BuildReport report   = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }

        EditorUserBuildSettings.buildAppBundle = false;
        buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "android/" + Application.productName + ".apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building Android APK");
        report  = BuildPipeline.BuildPlayer(buildPlayerOptions);
        summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }

        EditorApplication.Exit(0);
    }

    [MenuItem("Build/Build iOS")]
    public static void BuildIos()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "ios";
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building iOS");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built iOS");
    }

    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "windows/" + Application.productName + ".exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building StandaloneWindows64");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built StandaloneWindows64");
    }

    private static string[] GetScenes()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }

}