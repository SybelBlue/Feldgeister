using UnityEngine;

/// <summary> A morale behavior that does not decrease from max </summary>
[System.Serializable, CreateAssetMenu(fileName = "Unflinching Morale", menuName = "Feldgeister/Unflinching Morale")]
public class UnflinchingMorale : Morale
{
    public override MoraleLevel mood => MoraleLevel.Unflinching;
}
