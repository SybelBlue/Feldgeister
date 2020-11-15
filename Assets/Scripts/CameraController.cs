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

    [SerializeField, Range(0, 1)]
    private float scrollSensitivity = 0.1f;

    [SerializeField, ReadOnly]
    private float zoomValue = 1;

    [SerializeField]
    private float minZoomValue, maxZoomValue;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.frameCount % framesPerPan == 0)
        {
            PanWithMouse();
        }

        UpdateZoom();
    }

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

    private void UpdateZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        
        if (scroll == 0) return;

        zoomValue *= scroll > 0 ? 1 + scrollSensitivity : 1 - scrollSensitivity;
        zoomValue = Clamp(zoomValue, minZoomValue, maxZoomValue);

        transform.localScale = originalScale * zoomValue;
    }

    private void OnDrawGizmosSelected()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.yellow;
        
        var bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        var width = (topRight.x - bottomLeft.x) * pctHorizontalDeadzone / 100f;
        var height = (topRight.y - bottomLeft.y) * pctVerticalDeadzone / 100f;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));
        
        Gizmos.color = oldColor;
    }
}
