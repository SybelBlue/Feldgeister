using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "Terrorized Morale", menuName = "Feldgeister/Terrorized Morale")]
/// <summary> A morale behavior that does not increase from min </summary>
public class TerrorizedMorale : Morale
{
    public override MoraleLevel mood => MoraleLevel.Terrorized;
}