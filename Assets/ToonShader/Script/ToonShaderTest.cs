using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ToonShaderTestRig : MonoBehaviour
{
    [Range(1, 5)] public int bandCount = 3;
    [Range(0f, 1f)] public float specSize = 0.5f;
    public Color shadowTint = Color.black;

    [Header("Materials to drive")]
    public List<Material> targetMaterials = new();

    [Header("Global shader properties")]
    [Tooltip("Only has effect if the Blackboard properties are set to Unexposed.")]
    public bool pushGlobals = false;

    [Header("Safety")]
    [Tooltip("Material.SetFloat permanently edits the Material asset outside Play mode. Leave off unless you mean to.")]
    public bool applyToMaterialsInEditMode = false;

    static readonly int BandCountID = Shader.PropertyToID("_BandCount");
    static readonly int SpecSizeID = Shader.PropertyToID("_SpecSize");
    static readonly int ShadowTintID = Shader.PropertyToID("_ShadownTint"); // matches current Blackboard spelling

    void OnEnable() => PushAll();
    void OnValidate() => PushAll();
    void Update() => PushAll();

    public void PushAll()
    {
        if (pushGlobals)
            PushGlobals();

        if (Application.isPlaying || applyToMaterialsInEditMode)
            PushToMaterials();
    }

    void PushGlobals()
    {
        Shader.SetGlobalFloat(BandCountID, bandCount);
        Shader.SetGlobalFloat(SpecSizeID, specSize);
        Shader.SetGlobalColor(ShadowTintID, shadowTint);
    }

    void PushToMaterials()
    {
        foreach (var mat in targetMaterials)
        {
            if (mat == null) continue;
            mat.SetFloat(BandCountID, bandCount);
            mat.SetFloat(SpecSizeID, specSize);
            mat.SetColor(ShadowTintID, shadowTint);
        }
    }
}