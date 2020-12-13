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

        gameController.houseOccupantUI.UpdateDisplay(
            // shut off houseOccupantUI during dialogue
            runningDialogue ? null : c, 
            // can only talk if the character exists and hasn't talked already
            canTalkTo: c && !HasTalkedToday(c)
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

        var npcConor = character?.GetComponent<NPC_Conor>();
        if (!npcConor || !npcConor.enabled)
        {
            LogWarning("Rejected dialogue start, no active NPC_Conor found.");
            return;
        }

        npcConor.RunDialogue();
        gameController.runningDialogue = true;
        gameController.houseOccupantUI.UpdateDisplay(null);

        MarkTalkedToday(character);
    }

    private void MarkTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job] = true;
    
    public bool HasTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job];
}