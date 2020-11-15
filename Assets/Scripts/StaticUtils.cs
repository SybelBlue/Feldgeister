using UnityEngine;
using static UnityEngine.Input;

public static class StaticUtils 
{
    public static Vector3 currentWorldMousePosition 
        => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    public static bool LeftDown() => GetAxis("Horizontal") < 0;
    public static bool RightDown() => GetAxis("Horizontal") > 0;

}