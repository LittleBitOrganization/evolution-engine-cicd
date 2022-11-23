using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Serialization;

namespace LittleBit.Modules.CICD.Editor
{
    public class workflows
    {
        [YamlMember(Alias = "unity-android-workflow", ApplyNamingConventions = false)]
        public UnityAndroidWorkflow unityAndroidWorkflow { get; set; }
    }

    public class UnityAndroidWorkflow
    {
        public string name { get; set; }
        public string max_build_duration { get; set; }
        public string instance_type { get; set; }
        public Environment environment { get; set; }
        public Triggering triggering { get; set; }
        public List<Script> scripts { get; set; }
        public List<string> artifacts { get; set; }
        public Publishing publishing { get; set; }
    }
    
    public class Environment
    {
        public List<string> android_signing { get; set; }
        public List<string> groups { get; set; }
        public Vars vars { get; set; }
    }

    public class Vars
    {
        public string UNITY_BIN{ get; set; }
        public string UNITY_VERSION { get; set; }
        public string UNITY_VERSION_CHANGESET { get; set; }
        public string UNITY_VERSION_BIN { get; set; }
        public string BUILD_SCRIPT { get; set; }
        public string BUILD_NAME { get; set; }
        public string PACKAGE_NAME { get; set; }
    }

    public class Triggering
    {
        public List<string> events { get; set; }
        public List<BranchPatterns> branch_patterns { get; set; }
    }

    public class BranchPatterns
    {
        public string pattern { get; set; }
        public string include { get; set; }
        public string source { get; set; }
    }

    public class Cache
    {
        public List<string> cache_paths { get; set; }
    }
    
    public class Script
    {
        public string name { get; set; }
        public string script { get; set; }
    }

    public class Publishing
    {
        public List<Script> scripts { get; set; }
        public Email email { get; set; }
        public Slack slack { get; set; }
        public GooglePlay google_play { get; set; }
    }

    public class Email
    {
        public List<string> recipients { get; set; }
    }

    public class Slack
    {
        public string channel { get; set; }
        public string notify_on_build_start { get; set; }
    }
    
    public class GooglePlay
    {
        public string credentials { get; set; }
        public string track{ get; set; }
        public string in_app_update_priority { get; set; }
        public string changes_not_sent_for_review { get; set; }
        public string submit_as_draft { get; set; }
    }
}
