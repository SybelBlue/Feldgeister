using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplayController : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Image displayImage;

    [SerializeField, ReadOnly]
    private Character character;

    void OnValidate()
    {
        displayImage = GetComponent<Image>();
    }

    public void DisplayCharacter(Character c)
    {
        character = c;
        // set active if not null
        gameObject.SetActive(c);
    }

    void Update()
    {
        if (!character) return;
        displayImage.sprite = character.spriteForMood;
    }
}
