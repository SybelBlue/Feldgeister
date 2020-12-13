using static UnityEngine.Debug;

public class FoodSelectionMode : ISelectionMode
{
    public string name => "Food Donation";

    private GameController gameController;

    public FoodSelectionMode(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnBeginSelectionMode()
    {
        Log("Display: Highlight food?");
    }
    
    public void OnEndSelectionMode()
    {  }
    
    public void OnHover(Character character)
    {
        LogWarning($"Display: character hunger level is {character.hunger}");
    }
    
    public void OnSelected(Character character)
    {
        if (character && gameController.foodRemaining > 0 && character.Feed())
        {
            gameController.LoseFood();
        }
    }
}