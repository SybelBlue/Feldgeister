using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [ReadOnly, SerializeField]
    private Text text;

    void OnValidate()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        if (!text.text.StartsWith("> ") && !text.text.EndsWith(" <"))
        {
            text.text = $"> {text.text} <";
        }
    }
    
    public void OnPointerExit(PointerEventData data)
    { 
        if (text.text.StartsWith("> ") && text.text.EndsWith(" <"))
        {
            text.text = text.text.Substring(2, text.text.Length - 4);
        }
    }
}
