using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An event to fire when a global character event happens, like death
/// </summary>
[Serializable]
public class CharacterEvent : UnityEvent<Character>
{ }

/// <summary>
/// A class that contains all information about the characters.
/// Each character has an IMorale, a hunger stat, and a CharacterClass 
/// that determines their general behavior.
///
/// Only one Character script is allowed per object
/// </summary>
[Serializable, DisallowMultipleComponent]
public class Character : MonoBehaviour
{
    public CharacterClass characterClass;

    public House house;

    public CharacterEvent onDeath;

    public Sprite happySprite, normalSprite, angrySprite;

    public Sprite spriteForMood
    {
        get
        {
            switch(mood)
            {
                case MoraleLevel.Angry:
                    return angrySprite;
                case MoraleLevel.Joyous:
                    return happySprite;
                default:
                    return normalSprite;
            }
        }
    }

    public bool immortal {
        get => characterClass == CharacterClass.Mayor || characterClass == CharacterClass.Witch;
    }

    public bool alive;

    private HungerLevel _hunger;
    public HungerLevel hunger
    {
        get => characterClass == CharacterClass.Farmer ? 
            HungerLevel.Full :
            _hunger;
        set => _hunger = value;
    }

    public bool unchangingMood;
    
    [SerializeField]
    private MoraleLevel _mood;
    
    /// <summary> 
    /// If unchangingMood:
    ///     returns the stored mood, can be set and get normally.
    ///
    /// Else:
    ///     computes the current mood based on the current moraleValue,
    ///     cannot be set directly (will have no effect)
    /// </summary>
    public MoraleLevel mood
    {
        get => unchangingMood ? 
                _mood :
                moraleValue * 4 >= maxMorale * 3 ?
                    MoraleLevel.Joyous :
                    moraleValue * 4 <= maxMorale ?
                        MoraleLevel.Angry:
                        MoraleLevel.Normal;
        set => _mood = value;
    }

    [SerializeField]
    private int _moraleValue;
    
    public int moraleValue { 
        get => _moraleValue;
        set => _moraleValue = ClampMorale(value);
    }

    [SerializeField]
    private int _maxMorale;
    public int maxMorale 
    { 
        get => _maxMorale;
        set {
            _maxMorale = Mathf.Max(0, value);
            this.moraleValue = _moraleValue;
        }
    }

        /// <summary> increases morale by 1 </summary>
    public void IncreaseMorale()
    {
        if (unchangingMood) return;
        moraleValue = ClampMorale(moraleValue + 1);
    }
    
    /// <summary> decreases morale by 1 </summary>
    public void DecreaseMorale()
    {
        if (unchangingMood) return;
        moraleValue = ClampMorale(moraleValue - 1);
    }
    
    /// <summary> sets moraleValue to min </summary>
    public void PlummetMorale()
    {
        if (unchangingMood) return;
        moraleValue = 0;
    }

    /// <summary> sets moraleValue to max </summary>
    public void SkyrocketMorale()
    {
        if (unchangingMood) return;
        moraleValue = maxMorale;
    }

    /// <summary>
    /// returns newMorale if it's between 0 and maxMorale,
    /// otherwise it returns the closest bound to newMorale
    /// </summary>
    private int ClampMorale(int newMorale)
        => Mathf.Clamp(newMorale, 0, maxMorale);
}