using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshSerializer))]
public class MeshSerializerUI : Editor
{    
    MeshSerializer _target_;
    private void OnEnable()
    {
        _target_ = target as MeshSerializer;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Mesh to serialize");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("inputObject"));

        GUILayout.Label("Deserialized mesh object");
        
        if (GUILayout.Button("Serialize"))
        {
            _target_.SerializeModel();
        }
        GUILayout.Space(20);
        _target_.json_path = EditorGUILayout.TextField("JSON path", _target_.json_path);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("outputObject"));
        if (GUILayout.Button("Deserialize"))
        {
            _target_.DeserializeMesh();
        }

        GUILayout.Space(20);
        if (GUILayout.Button("Reset"))
        {
            
        }
        serializedObject.ApplyModifiedProperties();
    }
}
