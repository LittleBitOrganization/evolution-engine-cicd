using System.Collections.Generic;
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

        [Button()]
        public void EditYamlFile()
        {
            var deserializer = new DeserializerBuilder()
                .Build();
            var yamlObject = deserializer.Deserialize<YamlCICD>(YamlWrapper.GetTextYaml());

            yamlObject.workflows.unityAndroidWorkflow.environment.vars.PACKAGE_NAME =
                "com." + Application.companyName + "." + Application.productName;
            yamlObject.workflows.unityAndroidWorkflow.environment.android_signing = AndroidSigning;
            yamlObject.workflows.unityAndroidWorkflow.environment.vars.UNITY_VERSION_CHANGESET =
                UnityChangeset.GetUnityChangeset();
            
            if (AndroidSigning.Count > 0)
            {
                var serializer = new SerializerBuilder()
                    .Build();
                YamlWrapper.EditCopyYaml(serializer.Serialize(yamlObject));
            }
            else
            {
                Debug.LogError("AndroidSigning EMPTY!!!");
            }
        }
    }
}
