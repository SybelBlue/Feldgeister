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
    
    public void OnHover(Character c)
    {
        LogWarning($"Display: character hunger level is {c.hunger}");
    }
    
    public void OnSelected(Character c)
    {
        if (c && gameController.foodRemaining > 0 && c.Feed())
        {
            gameController.LoseFood();
        }
    }
}