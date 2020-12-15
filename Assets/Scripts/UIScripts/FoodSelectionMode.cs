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
        gameController.ClearHouseAndCharacterDisplays();
    }
    
    public void OnEndSelectionMode()
    {  }
    
    public void OnHover(Character character)
    {
        if (character)
        {
            Log($"TODO: Display: {character.job} hunger level is {character.hunger}");
        }
        gameController.houseOccupantUI.UpdateDisplay(
            gameController.runningDialogue ? null : character, 
            HouseOccupant.InstructionText.GiveFood
        );
        gameController.leftCharacterDisplay.UpdateDisplay(
            gameController.runningDialogue ? null : character);
    }
    
    public void OnSelected(Character character)
    {
        if (character && gameController.foodRemaining > 0 && character.Feed())
        {
            gameController.foodRemaining--;
        }
    }
}