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
    {
        gameController.ClearHouseAndCharacterDisplays();
        gameController.endDefensePlacement.SetActive(true);
    }
    
    public void OnEndSelectionMode()
    {  }
    
    public void OnHover(Character character)
    {
        if (character)
        {
            Log("TODO: Display in-depth defense stuff for " + character.job);
        }
    }
    
    public void OnSelected(Character character)
    {
        if (character)
        {
            LogWarning("TODO: Give " + character.job + " next defense");
            // character.house.defenses.Add(...)
        }
    }
}