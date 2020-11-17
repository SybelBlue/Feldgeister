using UnityEngine;

using static StaticUtils;
using static UnityEngine.Mathf;

#pragma warning disable 0649
public class CameraController : MonoBehaviour
{
    [Tooltip("Defines the outer limits of where the camera can move")]
    public Vector2Int mapDimensions;

    [SerializeField, Range(1, 100), Tooltip("Percent of the width of the window that does not pan when the mouse is present.")]
    private float pctHorizontalDeadzone = 60;
    [SerializeField, Range(1, 100), Tooltip("Percent of the height of the window that does not pan when the mouse is present.")]
    private float pctVerticalDeadzone = 60;
    
    [SerializeField, Range(1, 60), Tooltip("Number of frames that pass before checking to pan with mouse. (Higher value = Slower pan).")]
    private int framesPerPan = 10;

    [SerializeField, Range(0, 1)]
    private float scrollSensitivity = 0.1f;

    [SerializeField, ReadOnly]
    private float zoomValue;

    [SerializeField]
    private float minZoomValue, maxZoomValue;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        zoomValue = 1.5f;
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

        transform.position = transform.position.ClampInCube(
            new Vector3(-mapDimensions.x/2f, -mapDimensions.y/2f, -Mathf.Infinity),
            new Vector3(mapDimensions.x/2f, mapDimensions.y/2f, Mathf.Infinity)
        );
    }

    private void UpdateZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        
        if (scroll != 0)
        {
            zoomValue *= scroll > 0 ? 1 + scrollSensitivity : 1 - scrollSensitivity;
            zoomValue = Clamp(zoomValue, minZoomValue, maxZoomValue);
        }

        transform.localScale = originalScale * zoomValue;
    }

    private void OnDrawGizmosSelected()
    {
        var oldColor = Gizmos.color;
        Gizmos.color = Color.yellow;
        
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        float vwWidth = topRight.x - bottomLeft.x;
        float vwHeight = topRight.y - bottomLeft.y;
        float width = vwWidth * pctHorizontalDeadzone / 100f;
        float height = vwHeight * pctVerticalDeadzone / 100f;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(vwWidth, vwHeight, 1));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapDimensions.x, mapDimensions.y, 1));

        Gizmos.color = oldColor;
    }
}
