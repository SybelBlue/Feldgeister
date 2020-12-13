using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseOccupant : MonoBehaviour 
{
    [System.Serializable]
    public enum InstructionText 
    {
        CanTalk,
        AlreadyTalked,
        TooHungry,
    }
    public Text instructionText;
    public TMP_Text nameText;
    private Character last;
    private InstructionText lastInstruction;

    public void UpdateDisplay(Character character, InstructionText instruction=InstructionText.CanTalk){
        if (last == character && lastInstruction == instruction) return;

        lastInstruction = instruction;
        last = character;

        switch (lastInstruction)
        {
            case InstructionText.AlreadyTalked:
                instructionText.text = ">> Already Talked Today <<";
                break;
            case InstructionText.TooHungry:
                instructionText.text = ">> Too Hungry, Need Food <<";
                break;
            default:
                instructionText.text = "Click House to Talk!";
                break;
        }

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