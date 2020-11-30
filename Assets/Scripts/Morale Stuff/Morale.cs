/// <summary> An interface that governs all Morale behaviors </summary>
[System.Serializable]
public abstract class Morale : UnityEngine.ScriptableObject
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