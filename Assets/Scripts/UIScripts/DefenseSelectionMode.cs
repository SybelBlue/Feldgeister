using static UnityEngine.Debug;

public class DefenseSelectionMode : ISelectionMode
{
    public string name => "Lamb Placement";

    private GameController gameController;

    public DefenseSelectionMode(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnBeginSelectionMode()
        => gameController.ClearHouseAndCharacterDisplays();
    
    public void OnEndSelectionMode()
    {  }
    
    public void OnHover(Character character)
    {
        if (character)
        {
            Log("Display in-depth defense stuff for " + character.job);
        }
    }
    
    public void OnSelected(Character character)
    {
        if (character)
        {
            LogWarning("Give " + character.job + " next defense");
        }
    }
}