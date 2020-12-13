using static UnityEngine.Debug;

public class DialogueSelectionMode : ISelectionMode
{
    public string name => "Dialogue Selection";

    private static int jobCount = System.Enum.GetValues(typeof(CharacterJob)).Length;
    private bool[] charactersTalkedToday = new bool[jobCount];

    private bool runningDialogue => gameController.runningDialogue;

    private GameController gameController;
    public DialogueSelectionMode(GameController controller)
    {
        gameController = controller;
    }

    public void OnBeginSelectionMode()
    {
        gameController.houseOccupantUI.UpdateDisplay(null);
        gameController.leftCharacterDisplay.DisplayCharacter(null);
        gameController.rightCharacterDisplay.DisplayCharacter(null);
    }

    public void OnEndSelectionMode()
    { }

    public void OnHover(Character c)
    {
        gameController.rightCharacterDisplay.DisplayCharacter(
            runningDialogue ? 
                gameController.mayorCharacter : 
                null
        );

        if (!runningDialogue && c)
        {
            gameController.houseOccupantUI.UpdateDisplay(c, !HasTalkedToday(c));
            gameController.leftCharacterDisplay.DisplayCharacter(c);
        }
        else
        {
            gameController.houseOccupantUI.UpdateDisplay(null);
            if (!runningDialogue)
            {
                gameController.leftCharacterDisplay.DisplayCharacter(null);
            }
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

        MarkTalkedToday(character);
    }

    private void MarkTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job] = true;
    
    public bool HasTalkedToday(Character c)
        => charactersTalkedToday[(int)c.job];
}