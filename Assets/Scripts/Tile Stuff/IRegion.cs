using UnityEngine;

public interface IRegion
{
    RectInt region { get; set; }
    void OnHoverEnter();
    void OnHoverExit();
    void OnClick();
}