using UnityEngine;

public static class StaticUtils 
{
    private static float one_ish = Mathf.PI / 3f;
    // need to multiply by one_ish so that the numbers are floats.
    // Anything resembling an int will always return 46.75821.
    // god help me
    public static float PerlinNoise(float x, float y)
        => Mathf.PerlinNoise(x * one_ish, y * one_ish);

    public static bool randomBool => Random.value < 0.5f;
    public static int randomSign => randomBool? -1 : 1;

    public static void DrawGizmoWireBox(RectInt rect, Color boxColor)
        => DrawGizmoWireBox(rect, boxColor, Vector3.zero);

    public static void DrawGizmoWireBox(RectInt rect, Color boxColor, Vector3 offset)
    {
        var oldColor = Gizmos.color;
        Gizmos.color = boxColor;
        
        Vector3 size = rect.size.To3DFloat();
        Vector3 position = offset + rect.position.To3DFloat() + size / 2.0f;
        Gizmos.DrawWireCube(position, size + new Vector3(0, 0, 1));

        Gizmos.color = oldColor;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Scriptable Objects/Building Defense")]
    public static void CreateDefense()
    {
        UnityEditor.ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<BuildingDefense>(), "New Defense.asset");
    }
#endif
}

#if UNITY_EDITOR
public sealed class DisabledGroup : System.IDisposable
{
    public readonly bool disable;
    /// <summary>
    /// Creates an IDisposable that, if doDisable is true, will disable the editor
    /// on construction, then re-enable it on dispose.
    /// If doDisable is false, does nothing.
    /// </summary>
    public DisabledGroup(bool doDisable=true)
    {
        disable = doDisable;
        if (disable)
        {
            UnityEditor.EditorGUI.BeginDisabledGroup(true);
        }
    }
    
    public void Dispose()
    {
        if (disable)
        {
            UnityEditor.EditorGUI.EndDisabledGroup();
        }
    }
}
#endif