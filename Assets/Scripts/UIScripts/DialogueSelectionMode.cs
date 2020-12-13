using static UnityEngine.Debug;

public class DialogueSelectionMode : ISelectionMode
{
    public string name => "Dialogue Selection";

    private static int jobCount = System.Enum.GetValues(typeof(CharacterJob)).Length;
    private bool[] charactersTalkedToday;

    private bool runningDialogue => gameController.runningDialogue;

    private GameController gameController;

    public DialogueSelectionMode(GameController controller)
    {
        gameController = controller;
    }

    public void OnBeginSelectionMode()
    {
        charactersTalkedToday = new bool[jobCount];
        gameController.houseOccupantUI.UpdateDisplay(null);
        gameController.leftCharacterDisplay.DisplayCharacter(null);
        gameController.rightCharacterDisplay.DisplayCharacter(null);
    }

    public void OnEndSelectionMode()
    { }

    public void OnHover(Character c)
    {
        // only show the mayor during dialogue
        gameController.rightCharacterDisplay.DisplayCharacter(
            runningDialogue ? gameController.mayorCharacter : null
        );

        HouseOccupant.InstructionText instructionText = HouseOccupant.InstructionText.CanTalk;;
        if (c && HasTalkedToday(c)) {
            instructionText = HouseOccupant.InstructionText.AlreadyTalked;
        } else if (gameController.mayorCharacter.hunger == HungerLevel.Starving) { 
            instructionText = HouseOccupant.InstructionText.TooHungry;
        }

        gameController.houseOccupantUI.UpdateDisplay(
            // shut off houseOccupantUI during dialogue
            runningDialogue ? null : c, 
            instructionText
        );
        
        if (!runningDialogue) // freeze the display if running the dialogue
        {
            gameController.leftCharacterDisplay.DisplayCharacter(c);
        }
    }

    public void OnSelected(Character character)
    {
        if (!character)
        {
            LogWarning("Rejected dialogue start, null character.");
            return;
        }

        if (runningDialogue) 
        {
            LogWarning("Rejected dialogue start, dialogue already running.");
            return;
        }

        if (HasTalkedToday(character))
        {
            LogWarning("Rejected dialogue start, already talked today.");
            return;
        }

        if (gameController.mayorCharacter.hunger == HungerLevel.Starving)
        {
            LogWarning("Rejected dialogue start, mayor starving");
            return;
        }


        var npcConor = character?.GetComponent<NPC_Conor>();
        if (!npcConor || !npcConor.enabled)
        {
            LogWarning("Rejected dialogue start, no active NPC_Conor found.");
            return;
        }

        npcConor.RunDialogue();
        gameController.runningDialogue = true;
        gameController.houseOccupantUI.UpdateDisplay(null);

        if (gameController.foodRemaining > 0)
        {
            gameController.foodRemaining--;
        }
        else
        {
            gameController.mayorCharacter.GetHungrier();
        }

        MarkTalkedToday(character);
    }

    private void MarkTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job] = true;
    
    public bool HasTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job];
}