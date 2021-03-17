using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif

[ExecuteInEditMode]
public class BakeryVolume : MonoBehaviour
{
    public enum Encoding
    {
        // HDR L1 SH, half-float:
        // Tex0 = L0,  L1z.r
        // Tex1 = L1x, L1z.g
        // Tex2 = L1y, L1z.b
        Half4,

        // LDR L1 SH, 8-bit. Components are stored the same way as in Half4,
        // but L1 must be unpacked following way:
        // L1n = (L1n * 2 - 1) * L0 * 0.5 + 0.5
        RGBA8,

        // LDR L1 SH with monochrome directional component (= single color and direction), 8-bit.
        // Tex0 = L0    (alpha unused)
        // Tex1 = L1xyz (alpha unused)
        RGBA8Mono
    }

    public enum ShadowmaskEncoding
    {
        RGBA8,
        A8
    }

    public bool enableBaking = true;
    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
    public bool adaptiveRes = true;
    public float voxelsPerUnit = 0.5f;
    public int resolutionX = 16;
    public int resolutionY = 16;
    public int resolutionZ = 16;
    public Encoding encoding = Encoding.Half4;
    public ShadowmaskEncoding shadowmaskEncoding = ShadowmaskEncoding.RGBA8;
    public bool denoise = false;
    public bool isGlobal = false;
    public Texture3D bakedTexture0, bakedTexture1, bakedTexture2, bakedMask;

    public static BakeryVolume globalVolume;

    //public bool adjustSamples = true;

    public Vector3 GetMin()
    {
        return bounds.min;
    }

    public Vector3 GetInvSize()
    {
        var b = bounds;
        return new Vector3(1.0f/b.size.x, 1.0f/b.size.y, 1.0f/b.size.z);;
    }

    public void SetGlobalParams()
    {
        Shader.SetGlobalTexture("_Volume0", bakedTexture0);
        Shader.SetGlobalTexture("_Volume1", bakedTexture1);
        Shader.SetGlobalTexture("_Volume2", bakedTexture2);
        Shader.SetGlobalTexture("_VolumeMask", bakedMask);
        var b = bounds;
        var bmin = b.min;
        var bis = new Vector3(1.0f/b.size.x, 1.0f/b.size.y, 1.0f/b.size.z);;
        Shader.SetGlobalVector("_GlobalVolumeMin", bmin);
        Shader.SetGlobalVector("_GlobalVolumeInvSize", bis);
    }

    public void UpdateBounds()
    {
        var pos = transform.position;
        var size = bounds.size;
        bounds = new Bounds(pos, size);
    }

    public void Awake()
    {
        if (isGlobal)
        {
            globalVolume = this;
            SetGlobalParams();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BakeryVolume))]
public class BakeryVolumeInspector : Editor
{
    BoxBoundsHandle boundsHandle = new BoxBoundsHandle(typeof(BakeryVolumeInspector).GetHashCode());

    SerializedProperty ftraceAdaptiveRes, ftraceResX, ftraceResY, ftraceResZ, ftraceVoxelsPerUnit, ftraceAdjustSamples, ftraceEnableBaking, ftraceEncoding, ftraceShadowmaskEncoding, ftraceDenoise, ftraceGlobal;

    bool showExperimental = false;

    void OnEnable()
    {
        ftraceAdaptiveRes = serializedObject.FindProperty("adaptiveRes");
        ftraceVoxelsPerUnit = serializedObject.FindProperty("voxelsPerUnit");
        ftraceResX = serializedObject.FindProperty("resolutionX");
        ftraceResY = serializedObject.FindProperty("resolutionY");
        ftraceResZ = serializedObject.FindProperty("resolutionZ");
        ftraceEnableBaking = serializedObject.FindProperty("enableBaking");
        ftraceEncoding = serializedObject.FindProperty("encoding");
        ftraceShadowmaskEncoding = serializedObject.FindProperty("shadowmaskEncoding");
        ftraceDenoise = serializedObject.FindProperty("denoise");
        ftraceGlobal = serializedObject.FindProperty("isGlobal");
        //ftraceAdjustSamples = serializedObject.FindProperty("adjustSamples");
    }

    string F(float f)
    {
        // Unity keeps using comma for float printing on some systems since ~2018, even if system-wide decimal symbol is "."
        return (f + "").Replace(",", ".");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var vol = target as BakeryVolume;

        EditorGUILayout.PropertyField(ftraceEnableBaking, new GUIContent("Enable baking", "Should the volume be (re)computed? Disable to prevent overwriting existing data."));
        bool wasGlobal = ftraceGlobal.boolValue;
        EditorGUILayout.PropertyField(ftraceGlobal, new GUIContent("Global", "Automatically assign this volume to all volume-compatible shaders, unless they have overrides."));
        if (!wasGlobal && ftraceGlobal.boolValue)
        {
            (target as BakeryVolume).SetGlobalParams();
        }
        EditorGUILayout.PropertyField(ftraceDenoise, new GUIContent("Denoise", "Apply denoising after baking the volume."));
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(ftraceAdaptiveRes, new GUIContent("Adaptive resolution", "Calculate voxel resolution based on size?"));
        if (ftraceAdaptiveRes.boolValue)
        {
            EditorGUILayout.PropertyField(ftraceVoxelsPerUnit, new GUIContent("Voxels per unit"));

            GUI.enabled = false;
            var size = vol.bounds.size;
            ftraceResX.intValue = System.Math.Max((int)(size.x * vol.voxelsPerUnit), 1);
            ftraceResY.intValue = System.Math.Max((int)(size.y * vol.voxelsPerUnit), 1);
            ftraceResZ.intValue = System.Math.Max((int)(size.z * vol.voxelsPerUnit), 1);
        }
        EditorGUILayout.PropertyField(ftraceResX, new GUIContent("Resolution X"));
        EditorGUILayout.PropertyField(ftraceResY, new GUIContent("Resolution Y"));
        EditorGUILayout.PropertyField(ftraceResZ, new GUIContent("Resolution Z"));
        GUI.enabled = true;

        //EditorGUILayout.PropertyField(ftraceAdjustSamples, new GUIContent("Adjust sample positions", "Fixes light leaking from inside surfaces"));

        EditorGUILayout.Space();

        showExperimental = EditorGUILayout.Foldout(showExperimental, "Experimental", EditorStyles.foldout);
        if (showExperimental)
        {
            EditorGUILayout.PropertyField(ftraceEncoding, new GUIContent("Encoding"));
            EditorGUILayout.PropertyField(ftraceShadowmaskEncoding, new GUIContent("Shadowmask Encoding"));
        }

        EditorGUILayout.Space();

        if (vol.bakedTexture0 == null)
        {
            EditorGUILayout.LabelField("Baked texture: none");
        }
        else
        {
            EditorGUILayout.LabelField("Baked texture: " + vol.bakedTexture0.name);
        }

        EditorGUILayout.Space();

        var wrapObj = EditorGUILayout.ObjectField("Wrap to object", null, typeof(GameObject), true) as GameObject;
        if (wrapObj != null)
        {
            var mrs = wrapObj.GetComponentsInChildren<MeshRenderer>() as MeshRenderer[];
            if (mrs.Length > 0)
            {
                var b = mrs[0].bounds;
                for(int i=1; i<mrs.Length; i++)
                {
                    b.Encapsulate(mrs[i].bounds);
                }
                Undo.RecordObject(vol, "Change Bounds");
                vol.transform.position = b.center;
                vol.bounds = b;
                Debug.Log("Bounds set");
            }
            else
            {
                Debug.LogError("No mesh renderers to wrap to");
            }
        }

        var boxCol = vol.GetComponent<BoxCollider>();
        if (boxCol != null)
        {
            if (GUILayout.Button("Set from box collider"))
            {
                Undo.RecordObject(vol, "Change Bounds");
                vol.bounds = boxCol.bounds;
            }
            if (GUILayout.Button("Set to box collider"))
            {
                boxCol.center = Vector3.zero;
                boxCol.size = vol.bounds.size;
            }
        }

        var bmin = vol.bounds.min;
        var bmax = vol.bounds.max;
        var bsize = vol.bounds.size;
        EditorGUILayout.LabelField("Min: " + bmin.x+", "+bmin.y+", "+bmin.z);
        EditorGUILayout.LabelField("Max: " + bmax.x+", "+bmax.y+", "+bmax.z);

        if (GUILayout.Button("Copy bounds to clipboard"))
        {
            GUIUtility.systemCopyBuffer = "float3 bmin = float3(" + F(bmin.x)+", "+F(bmin.y)+", "+F(bmin.z) + "); float3 bmax = float3(" + F(bmax.x)+", "+F(bmax.y)+", "+F(bmax.z) + "); float3 binvsize = float3(" + F(1.0f/bsize.x)+", "+F(1.0f/bsize.y)+", "+F(1.0f/bsize.z) + ");";
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnSceneGUI()
    {
        var vol = (BakeryVolume)target;

        boundsHandle.center = vol.transform.position;
        boundsHandle.size = vol.bounds.size;

        EditorGUI.BeginChangeCheck();
        boundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(vol, "Change Bounds");

            Bounds newBounds = new Bounds();
            newBounds.center = boundsHandle.center;
            newBounds.size = boundsHandle.size;
            vol.bounds = newBounds;
            vol.transform.position = boundsHandle.center;
        }
        else if ((vol.bounds.center - boundsHandle.center).sqrMagnitude > 0.0001f)
        {
            Bounds newBounds = new Bounds();
            newBounds.center = boundsHandle.center;
            newBounds.size = boundsHandle.size;
            vol.bounds = newBounds;
        }
    }
}
#endif
