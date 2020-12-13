public interface ISelectionMode
{
    string name { get; }
    void OnBeginSelectionMode();
    void OnEndSelectionMode();
    void OnHover(Character c);
    void OnSelected(Character c);
}