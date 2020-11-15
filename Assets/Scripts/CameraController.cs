using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static StaticUtils;
using static UnityEngine.Mathf;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(1, 100), Tooltip("Percent of the width of the window that does not pan when the mouse is present.")]
    private float pctHorizontalDeadzone = 60;
    [SerializeField, Range(1, 100), Tooltip("Percent of the height of the window that does not pan when the mouse is present.")]
    private float pctVerticalDeadzone = 60;
    
    [SerializeField, Range(1, 60), Tooltip("Number of frames that pass before checking to pan with mouse. (Higher value = Slower pan).")]
    private int framesPerPan = 10;

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % framesPerPan == 0)
        {
            PanWithMouse();
        }
    }

    private Vector3 VwToWorld(float x, float y)
        => Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));

    private void PanWithMouse()
    {
        // each value is a percent of the height/width of the viewport
        // where the mouse is located relative to the lower left corner
        Vector3 pctMouseVwPos = 100 * Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // if mouse is not on the game viewport, don't pan.
        if (pctMouseVwPos.AnyComponent(v => v < 0 || v > 100)) return;

        // general formula:
        // | percentViewport - 50 | > 0.5 * percentDeadzone
        // returns true when the mouse is outside the deadzone in one axis


        if (Abs(pctMouseVwPos.x - 50) > pctHorizontalDeadzone / 2f)
        {
            transform.position += pctMouseVwPos.x < 50 ? Vector3.left : Vector3.right;
        }
        
        if (Abs(pctMouseVwPos.y - 50) > pctVerticalDeadzone / 2f)
        {
            transform.position += pctMouseVwPos.y < 50 ? Vector3.down : Vector3.up;
        }

    }
}
