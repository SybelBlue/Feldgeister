using UnityEngine;

/// <summary> An enum that generally categorizes morale levels </summary>
[System.Serializable]
public enum MoraleLevel
{
    Normal,
    Unflinching,
    Terrorized
}

/// <summary> An interface that governs all Morale behaviors </summary>
[System.Serializable]
public abstract class Morale : ScriptableObject
{
    /// <summary> returns general behavioral mood </summary>
    public abstract MoraleLevel mood { get; }

    /// <summary> increases morale </summary>
    public virtual void Increase()
    {  }
    
    /// <summary> decreases morale </summary>
    public virtual void Decrease()
    {  }
    
    /// <summary> plummets morale </summary>
    public virtual void Plummet()
    {  }

    /// <summary> skyrockets morale </summary>
    public virtual void Skyrocket()
    {  }
}

/// <summary> A morale behavior that does not decrease from max </summary>
public class UnflinchingMorale : Morale
{
    public override MoraleLevel mood => MoraleLevel.Unflinching;
}

/// <summary> A morale behavior that does not increase from min </summary>
public class TerrorizedMorale : Morale
{
    public override MoraleLevel mood => MoraleLevel.Terrorized;
}

/// <summary> The basic morale behavior class </summary>
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