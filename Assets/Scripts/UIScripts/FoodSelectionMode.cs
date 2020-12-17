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
        gameController.endDialogue.SetActive(false);
        gameController.foodDonationScreen.SetActive(true);
        gameController.endFoodDonation.SetActive(true);
    }
    
    public void OnEndSelectionMode()
    { 
        gameController.foodDonationScreen.SetActive(false);
        gameController.endFoodDonation.SetActive(false);
    }
    
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
            if (gameController.foodRemaining == 0)
            {
                gameController.FinishFoodSelection();
            }
        }
    }
}