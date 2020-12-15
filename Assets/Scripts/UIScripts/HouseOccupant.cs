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
        CanPlaceLamb,
        NotSuitableForLamb,
        GiveFood,
    }
    public Text instructionText;
    public TMP_Text nameText;
    private Character last;
    private InstructionText lastInstruction;

    public void UpdateDisplay(Character character, InstructionText instruction=InstructionText.CanTalk){
        if (last == character && lastInstruction == instruction) return;

        lastInstruction = instruction;
        last = character;

        instructionText.text = InstructionToString(instruction);

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

    public static string InstructionToString(InstructionText text)
    {
        switch (text)
        {
            // dialogue selection
            case InstructionText.AlreadyTalked:
                return ">> Already Talked Today <<";
            case InstructionText.TooHungry:
                return ">> Too Hungry, Need Food <<";
            case InstructionText.CanTalk:
                return "Click House to Talk!";
            // lamb placement
            case InstructionText.NotSuitableForLamb:
                return ">> Not Suitable for Lamb <<";
            case InstructionText.CanPlaceLamb:
                return "Click House to Place Lamb!";
            // food placement
            case InstructionText.GiveFood:
                return "Click House to Give Food!";
        }
        Debug.LogError($"Unknown instruction text! ({text})");
        return text.ToString();
    }
}