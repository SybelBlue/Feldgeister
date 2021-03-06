using UnityEngine;

public class BuildingDefense : ScriptableObject
{
    public string itemName = "New Defense";
    public string description;
    public bool magical, damaged;
    public int value = 1;
    public CharacterJob source;

    void OnValidate()
    {
        name = itemName;
    }
}