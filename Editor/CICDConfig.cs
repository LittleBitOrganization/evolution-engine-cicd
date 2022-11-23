using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;
using SerializerBuilder = YamlDotNet.Serialization.SerializerBuilder;

namespace LittleBit.Modules.CICD.Editor
{
    public class CICDConfig : ScriptableObject
    {
        [ResizableTextArea][ReadOnly] public string description = "AndroidSigning взять из Codemagic.io";
        [field: SerializeField] public List<string> AndroidSigning { get; private set; }

        [Dropdown("_instanceTypes")] public InstancesType enumInstance;
        [Dropdown("_isPublishingInGoogle")] public bool publishingInGoogle;
        [Dropdown("_unityVersions")] public UnityVersions unityVersions;
        private DropdownList<InstancesType> _instanceTypes()
        {
            return new DropdownList<InstancesType>()
            {
                { "Mac PRO", InstancesType.MacPro},
                { "Mac M1", InstancesType.MacM1},
                { "Mac Intel", InstancesType.MacIntel},
            };
        }
        
        private DropdownList<bool> _isPublishingInGoogle()
        {
            return new DropdownList<bool>()
            {
                { "true", true},
                { "false", false},
            };
        }

        public enum InstancesType
        {
            MacPro,
            MacM1,
            MacIntel
        }
        
        private DropdownList<UnityVersions> _unityVersions()
        {
            return new DropdownList<UnityVersions>
            {
                { "2023.1.0a19", new()
                {
                    UnityVersion = "2023.1.0a19",
                    UnityChangeSet = "0c2ff406cf78"
                }},
                { "2021.3.1f1", new()
                {
                    UnityVersion = "2021.3.1f1",
                    UnityChangeSet = "3b70a0754835"
                }},
                { "2022.1.23f1", new()
                {
                    UnityVersion = "2022.1.23f1",
                    UnityChangeSet = "9636b062134a"
                }},
                { "2022.2.0b16", new()
                {
                    UnityVersion = "2022.2.0b16",
                    UnityChangeSet = "3c3b3e6cd1d7"
                }},
            };
        }

        
        [Serializable]
        public struct UnityVersions
        {
            public string UnityVersion;
            public string UnityChangeSet;
        }
        
        private YamlCICD _mainYamlObject;
        private List<string> _removeObj;
        [Button()]
        public void EditYamlFile()
        {
            _removeObj = new();
            var deserializer = new DeserializerBuilder()
                .Build();
            _mainYamlObject = deserializer.Deserialize<YamlCICD>(YamlWrapper.GetTextYaml());

            SetDeactivateLicenseScriptToM1OrIntel();
            
            switch (enumInstance)
            {
                case InstancesType.MacPro:
                    _mainYamlObject.workflows.unityAndroidWorkflow.instance_type = "mac_pro";
                    break;
                case InstancesType.MacM1:
                    _mainYamlObject.workflows.unityAndroidWorkflow.instance_type = "mac_mini_m1";
                    break;
                case InstancesType.MacIntel:
                    _mainYamlObject.workflows.unityAndroidWorkflow.instance_type = "mac_mini";
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetGooglePublishing(publishingInGoogle);
            
            _mainYamlObject.workflows.unityAndroidWorkflow.environment.vars.PACKAGE_NAME = Application.identifier;
            _mainYamlObject.workflows.unityAndroidWorkflow.environment.android_signing = AndroidSigning;
            //_mainYamlObject.workflows.unityAndroidWorkflow.environment.vars.UNITY_VERSION_CHANGESET = UnityChangeset.GetUnityChangeset();

            _mainYamlObject.workflows.unityAndroidWorkflow.environment.vars.UNITY_VERSION = unityVersions.UnityVersion;
            _mainYamlObject.workflows.unityAndroidWorkflow.environment.vars.UNITY_VERSION_CHANGESET = unityVersions.UnityChangeSet;
            
            if (AndroidSigning.Count > 0)
            {
                var serializer = new SerializerBuilder()
                    .Build();

                YamlWrapper.EditCopyYaml(serializer.Serialize(_mainYamlObject), _removeObj);
                YamlWrapper.CopySSH();
            }
            else
            {
                Debug.LogError("AndroidSigning EMPTY!!!");
            }
        }

        private void SetGooglePublishing(bool b)
        {
            if (b)
            {
                foreach (var scripts in _mainYamlObject.workflows.unityAndroidWorkflow.scripts.Where(scripts => scripts.name.Contains("Unity build")))
                {
                    scripts.script = "export NEW_BUILD_NUMBER=$(($(google-play get-latest-build-number --tracks \"alpha\" --package-name \"$PACKAGE_NAME\") + 1))\n$UNITY_VERSION_BIN -batchmode -projectPath . -executeMethod BuildScript.$BUILD_SCRIPT -nographics -buildTarget Android > $CM_BUILD_DIR/buildAndroid.log 2>&1"; 
                }
            }
            else
            {
                _removeObj.Add("google_play:");
                foreach (var scripts in _mainYamlObject.workflows.unityAndroidWorkflow.scripts.Where(scripts => scripts.name.Contains("Unity build")))
                {
                    scripts.script = "$UNITY_VERSION_BIN -batchmode -projectPath . -executeMethod BuildScript.$BUILD_SCRIPT -nographics -buildTarget Android > $CM_BUILD_DIR/buildAndroid.log 2>&1";
                }
            }
            
        }

        private void SetDeactivateLicenseScriptToM1OrIntel()
        {
            foreach (var scripts in _mainYamlObject.workflows.unityAndroidWorkflow.publishing.scripts.Where(scripts => scripts.name.Contains("Deactivate Unity License")))
            {
                scripts.script = enumInstance is InstancesType.MacM1 or InstancesType.MacPro ? 
                    "/Applications/Unity\\ Hub.app/Contents/Frameworks/UnityLicensingClient_V1.app/Contents/MacOS/Unity.Licensing.Client --return-ulf --username ${UNITY_EMAIL?} --password ${UNITY_PASSWORD?}" : 
                    "$UNITY_BIN -batchmode -quit -returnlicense -nographics";
            }
        }
    }
}
