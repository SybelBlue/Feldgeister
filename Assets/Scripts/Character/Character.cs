using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public CharacterJob job;

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

    public bool immortal
        => job == CharacterJob.Mayor || job == CharacterJob.Witch;

    [SerializeField]
    private bool _alive = true;
    public bool alive
    {
        get => _alive;
        set
        {
            if (!immortal && _alive && !value)
            {
                onDeath.Invoke(this);
            }

            _alive = immortal || value;
        }
    }

    [SerializeField]
    private HungerLevel _hunger;
    public HungerLevel hunger
    {
        get => job == CharacterJob.Farmer ? 
            HungerLevel.Full :
            _hunger;
        set => _hunger = value;
    }

    /// <summary> returns true when character eats, false otherwise </summary>
    public bool Feed()
    {
        if (hunger == HungerLevel.Full) return false;
        print($"Feeding {job}");
        hunger = (HungerLevel)Mathf.Clamp((int)hunger + 1, (int)HungerLevel.Starving, (int)HungerLevel.Full);
        return true;
    }

    public void GetHungrier()
    {
        if (hunger == HungerLevel.Starving) return;
        hunger = (HungerLevel)Mathf.Clamp((int)hunger - 1, (int)HungerLevel.Starving, (int)HungerLevel.Full);
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

#if UNITY_EDITOR
[CustomEditor(typeof(Character)), CanEditMultipleObjects]
public class CharacterEditor : Editor
{
    private SerializedProperty onDeathProp;
    private SerializedProperty happySpriteProp;
    private SerializedProperty normalSpriteProp;
    private SerializedProperty angrySpriteProp;

    private Character character 
        => target as Character;

    void OnEnable()
    {
        onDeathProp = serializedObject.FindProperty("onDeath");
        happySpriteProp = serializedObject.FindProperty("happySprite");
        normalSpriteProp = serializedObject.FindProperty("normalSprite");
        angrySpriteProp = serializedObject.FindProperty("angrySprite");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        character.name = character.job.ToString();

        character.job = (CharacterJob)EditorGUILayout.EnumPopup("Job", character.job);

        EditorGUILayout.Space();

        using (new DisabledGroup(character.immortal))
        {
            character.alive = EditorGUILayout.Toggle("Alive", character.immortal || character.alive);
        }

        if (character.alive)
        {
            EditorGUILayout.Space();

            // if farmer, can't change hunger level from max
            using (new DisabledGroup(character.job == CharacterJob.Farmer))
            {
                character.hunger = (HungerLevel)EditorGUILayout.EnumPopup("Hunger", character.hunger);
            }

            EditorGUILayout.Space();

            // make checkbox for unchanging mood
            character.unchangingMood = EditorGUILayout.Toggle("Unchanging Mood", character.unchangingMood);
            
            // if character has unchanging mood, then can't change mood
            using (new DisabledGroup(character.unchangingMood))
            {
                character.mood = (MoraleLevel)EditorGUILayout.EnumPopup("Mood", character.mood);
            }
            
            // make sub fields for altering the morale levels
            if (!character.unchangingMood)
            {
                EditorGUI.indentLevel++;

                // set the max morale in [3, 10]
                character.maxMorale = EditorGUILayout.IntSlider("Max Morale", Mathf.Clamp(character.maxMorale, 3, 10), 3, 10);
                // set the morale value in [0, maxMorale]
                character.moraleValue = EditorGUILayout.IntSlider("Morale", character.moraleValue, 0, character.maxMorale);

                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(happySpriteProp, new GUIContent("Happy Sprite"));
        EditorGUILayout.PropertyField(normalSpriteProp, new GUIContent("Normal Sprite"));
        EditorGUILayout.PropertyField(angrySpriteProp, new GUIContent("Angry Sprite"));

        EditorGUILayout.Separator();

        // create the onDeath event box
        EditorGUILayout.PropertyField(onDeathProp, new GUIContent(character.immortal ? "On \"Death\" <immortal>" : "On Death"));

        EditorGUILayout.Separator();

        // serialize changes, if any made
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(character);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif