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
}