using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToonShaderTestRig))]
public class ToonShaderTestRigEditor : Editor
{
    SerializedProperty bandCount, specSize, shadowTint, targetMaterials, pushGlobals, applyInEdit;

    void OnEnable()
    {
        bandCount = serializedObject.FindProperty("bandCount");
        specSize = serializedObject.FindProperty("specSize");
        shadowTint = serializedObject.FindProperty("shadowTint");
        targetMaterials = serializedObject.FindProperty("targetMaterials");
        pushGlobals = serializedObject.FindProperty("pushGlobals");
        applyInEdit = serializedObject.FindProperty("applyToMaterialsInEditMode");
    }

    public override void OnInspectorGUI()
    {
        var rig = (ToonShaderTestRig)target;

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.IntSlider(bandCount, 1, 5, "Band Count");
        EditorGUILayout.Slider(specSize, 0f, 1f, "Spec Size");
        EditorGUILayout.PropertyField(shadowTint, new GUIContent("Shadow Tint"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(targetMaterials, new GUIContent("Target Materials"), true);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(pushGlobals, new GUIContent("Push Global Shader Values"));
        EditorGUILayout.PropertyField(applyInEdit, new GUIContent("Apply To Materials In Edit Mode"));
        if (applyInEdit.boolValue && !Application.isPlaying)
            EditorGUILayout.HelpBox("This will permanently modify the Material assets in the list right now, not just at runtime.", MessageType.Warning);

        bool changed = EditorGUI.EndChangeCheck();
        serializedObject.ApplyModifiedProperties();

        if (changed)
            rig.PushAll();

        EditorGUILayout.Space();
        if (GUILayout.Button("Push Now"))
            rig.PushAll();
    }
}