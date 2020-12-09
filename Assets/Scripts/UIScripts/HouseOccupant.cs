using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseOccupant : MonoBehaviour 
{
    public Text instructionText;
    public TMP_Text nameText;
    private Character last;
    private bool lastCanTalk;

    public void UpdateDisplay(Character character, bool canTalkTo=true){
        if (last == character && lastCanTalk == canTalkTo) return;

        lastCanTalk = canTalkTo;
        last = character;

        instructionText.text = canTalkTo ? "Click house to talk" : ">> Already talked today! <<";

        if(character) 
        {
            this.gameObject.SetActive(true);
            nameText.text = character.name;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}