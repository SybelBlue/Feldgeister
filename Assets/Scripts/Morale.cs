using UnityEngine;

public interface IMorale
{
    bool unflinching { get; }
    bool terrorized { get; }

    void Increase();
    void Decrease();
    void Plummet();
    void Skyrocket();
}

public class StandardMorale : IMorale
{
    public bool unflinching => morale * 4 >= maxMorale * 3;
    public bool terrorized => morale * 4 <= maxMorale;

    private short _morale;
    public short morale { 
        get => _morale;
        set => _morale = ClampMorale(value);
    }
    
    private short _maxMorale;
    public short maxMorale 
    { 
        get => _maxMorale;
        set {
            _maxMorale = (short)Mathf.Max(0, value);
            morale = _morale;
        }
    }

    public StandardMorale(short maxMorale = 6)
    {
        this.maxMorale = maxMorale;
        morale = (short)UnityEngine.Random.Range(maxMorale / 2, maxMorale);
    }

    public void Increase() 
        => morale = ClampMorale(morale - 1);

    public void Decrease()
        => morale = ClampMorale(morale + 1);

    public void Plummet()
        => morale = 0;

    public void Skyrocket()
        => morale = maxMorale;

    private short ClampMorale(int newMorale)
        => (short)Mathf.Clamp(newMorale, 0, maxMorale);
}

public class UnflinchingMorale : IMorale
{  
    public bool unflinching => true;
    public bool terrorized => false;

    public void Increase() 
    {  }

    public void Decrease()
    {  }

    public void Plummet() 
    {  }

    public void Skyrocket()
    {  }
}

public class TerrorizedMorale : IMorale
{
    public bool unflinching => false;
    public bool terrorized => true;

    public void Increase() 
    {  }

    public void Decrease()
    {  }

    public void Plummet() 
    {  }

    public void Skyrocket()
    {  }
}