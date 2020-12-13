using static UnityEngine.Debug;

public class LambSelectionMode : ISelectionMode
{
    public string name => "Lamb Placement";

    private GameController gameController;

    public LambSelectionMode(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnBeginSelectionMode()
        => gameController.ClearHouseAndCharacterDisplays();
    
    public void OnEndSelectionMode()
        => gameController.ClearHouseAndCharacterDisplays();
    
    public void OnHover(Character character)
    {
        gameController.houseOccupantUI.UpdateDisplay(
            character,
            character && character.canTakeLamb ? 
                HouseOccupant.InstructionText.CanPlaceLamb :
                HouseOccupant.InstructionText.NotSuitableForLamb
        );

        gameController.leftCharacterDisplay.UpdateDisplay(character);
    }
    
    public void OnSelected(Character character)
    {
        if (character && character.canTakeLamb)
        {
            character.house.hasLamb = true;
            gameController.FinishLambSelection();
        }
    }
}