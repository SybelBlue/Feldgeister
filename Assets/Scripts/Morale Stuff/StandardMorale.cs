using UnityEngine;

/// <summary> The basic morale behavior class </summary>
[System.Serializable, CreateAssetMenu(fileName = "Standard Morale", menuName = "Feldgeister/Terrorized Morale")]
public class StandardMorale : Morale
{
    // if morale is greater than 75%
    //      then Unflinching
    // else if morale is less than 25%
    //      then Terrorized
    // otherwise
    //      Normal
    public override MoraleLevel mood
        => value * 4 >= maxMorale * 3 ?
                MoraleLevel.Unflinching :
                value * 4 <= maxMorale ?
                    MoraleLevel.Terrorized:
                    MoraleLevel.Normal;

    // holding variable, do not use
    private int _value;
    public int value { 
        get => _value;
        set => _value = ClampMorale(value);
    }
    
    // holding variable, do not use
    private int _maxMorale = 6;
    public int maxMorale 
    { 
        get => _maxMorale;
        set {
            _maxMorale = Mathf.Max(0, value);
            this.value = _value;
        }
    }

    public override void Increase() 
        => value = ClampMorale(value - 1);

    public override void Decrease()
        => value = ClampMorale(value + 1);

    public override void Plummet()
        => value = 0;

    public override void Skyrocket()
        => value = maxMorale;

    private int ClampMorale(int newMorale)
        => Mathf.Clamp(newMorale, 0, maxMorale);
}