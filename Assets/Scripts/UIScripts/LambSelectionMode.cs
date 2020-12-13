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
    {  }
    
    public void OnEndSelectionMode()
    {  }
    
    public void OnHover(Character character)
    {
        gameController.houseOccupantUI.UpdateDisplay(
            character,
            character && character.canTakeLamb ? 
                HouseOccupant.InstructionText.CanPlaceLamb :
                HouseOccupant.InstructionText.NotSuitableForLamb
        );

        gameController.leftCharacterDisplay.DisplayCharacter(character);
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