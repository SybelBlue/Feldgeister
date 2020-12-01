using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    [ReadOnly, SerializeField]
    private Text text;

    [ReadOnly, SerializeField]
    private bool hovering = false;

    private GameController _gameController;
    private GameController gameController
        => (_gameController = _gameController ?? GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameController>());

    void OnValidate()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }
    
    public void OnPointerEnter(PointerEventData data)
    {
        if (hovering) return;

        hovering = true;

        if (text.text.StartsWith("> ") || text.text.EndsWith(" <")) return;

        text.text = $"> {text.text} <";
        gameController?.PlayButttonHover();
    }
    
    public void OnPointerExit(PointerEventData data)
    { 
        if (hovering && text.text.StartsWith("> ") && text.text.EndsWith(" <"))
        {
            text.text = text.text.Substring(2, text.text.Length - 4);
        }

        hovering = false;
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (hovering)
        {
            gameController?.PlayButttonSelect();
        }
    }

    void OnDisable()
    {
        hovering = false;
    }
}
