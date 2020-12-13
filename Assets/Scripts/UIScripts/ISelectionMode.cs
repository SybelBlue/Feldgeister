public interface ISelectionMode
{
    string name { get; }
    void OnBeginSelectionMode();
    void OnEndSelectionMode();
    void OnSelected();
}

public class DialogueSelectionMode : ISelectionMode
{
    string name => "Dialogue Selection";

    public void OnBeginSelectionMode()
    {

    }

    public void OnEndSelectionMode()
    {
        
    }

    public void OnSelected()
    {
        
    }
}