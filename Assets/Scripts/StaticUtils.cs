using UnityEngine;
using static UnityEngine.Input;

public static class StaticUtils 
{
    public static Vector3 currentWorldMousePosition 
        => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    public static bool LeftDown() => GetAxis("Horizontal") < 0;
    public static bool RightDown() => GetAxis("Horizontal") > 0;


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
}