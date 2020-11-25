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
public interface IMorale
{
    /// <summary> returns general behavioral mood </summary>
    MoraleLevel mood { get; }

    /// <summary> increases morale </summary>
    void Increase();
    
    /// <summary> decreases morale </summary>
    void Decrease();
    
    /// <summary> plummets morale </summary>
    void Plummet();

    /// <summary> skyrockets morale </summary>
    void Skyrocket();
}

/// <summary> The basic morale behavior class </summary>
[System.Serializable]
public class StandardMorale : IMorale
{
    // if morale is greater than 75%
    //      then Unflinching
    // else if morale is less than 25%
    //      then Terrorized
    // otherwise
    //      Normal
    public MoraleLevel mood
        => value * 4 >= maxMorale * 3 ?
                MoraleLevel.Unflinching :
                value * 4 <= maxMorale ?
                    MoraleLevel.Terrorized:
                    MoraleLevel.Normal;

    // holding variable, do not use
    private short _value;
    public short value { 
        get => _value;
        set => _value = ClampMorale(value);
    }
    
    // holding variable, do not use
    private short _maxMorale;
    public short maxMorale 
    { 
        get => _maxMorale;
        set {
            _maxMorale = (short)Mathf.Max(0, value);
            this.value = _value;
        }
    }

    // creates a new basic morale behavior component
    public StandardMorale(short maxMorale = 6)
    {
        this.maxMorale = maxMorale;
        value = (short)UnityEngine.Random.Range(maxMorale / 2, maxMorale);
    }

    public void Increase() 
        => value = ClampMorale(value - 1);

    public void Decrease()
        => value = ClampMorale(value + 1);

    public void Plummet()
        => value = 0;

    public void Skyrocket()
        => value = maxMorale;

    private short ClampMorale(int newMorale)
        => (short)Mathf.Clamp(newMorale, 0, maxMorale);
}

/// <summary> A morale behavior that does not decrease from max </summary>
[System.Serializable]
public class UnflinchingMorale : IMorale
{
    public MoraleLevel mood => MoraleLevel.Unflinching;   

    public void Increase() 
    {  }

    public void Decrease()
    {  }

    public void Plummet() 
    {  }

    public void Skyrocket()
    {  }
}

/// <summary> A morale behavior that does not increase from min </summary>
[System.Serializable]
public class TerrorizedMorale : IMorale
{
    public MoraleLevel mood => MoraleLevel.Terrorized;

    public void Increase() 
    {  }

    public void Decrease()
    {  }

    public void Plummet() 
    {  }

    public void Skyrocket()
    {  }
}