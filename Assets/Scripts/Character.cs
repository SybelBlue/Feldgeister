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
    public CharacterClass characterClass;

    public CharacterEvent onDeath;

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
                    MoraleLevel.Unflinching :
                    moraleValue * 4 <= maxMorale ?
                        MoraleLevel.Terrorized:
                        MoraleLevel.Normal;
        set => _mood = value;
    }

    [SerializeField]
    private int _moraleValue;
    
    public int moraleValue { 
        get => _moraleValue;
        set => _moraleValue = ClampMorale(moraleValue);
    }

    [SerializeField]
    private int _maxMorale = 6;
    public int maxMorale 
    { 
        get => _maxMorale;
        set {
            _maxMorale = Mathf.Max(0, moraleValue);
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


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//                                FROM HERE ON IS STUFF FOR VISUALIZING THE 
//                                    CHARACTER CLASS IN THE UNITY INSPECTOR.
//                                         WILL NOT EFFECT GAMEPLAY AT RUNTIME

//                                  !!!!!!!!!        UNDOCUMENTED         !!!!!!!!!!!

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#if UNITY_EDITOR
[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    private SerializedProperty onDeathProp;

    private Character character {
        get => target as Character;
    }

    void OnEnable()
    {
        onDeathProp = serializedObject.FindProperty("onDeath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        
        character.name = character.characterClass.ToString();

        character.characterClass = (CharacterClass)EditorGUILayout.EnumPopup("Class", character.characterClass);

        EditorGUILayout.Space();

        if (character.immortal) EditorGUI.BeginDisabledGroup(true);
        character.alive = EditorGUILayout.Toggle("Alive", character.immortal || character.alive);
        if (character.immortal) EditorGUI.EndDisabledGroup();

        if (character.alive)
        {
            EditorGUILayout.Space();

            if (character.characterClass == CharacterClass.Farmer) EditorGUI.BeginDisabledGroup(true);
            character.hunger = (HungerLevel)EditorGUILayout.EnumPopup("Hunger", character.hunger);
            if (character.characterClass == CharacterClass.Farmer) EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            character.unchangingMood = EditorGUILayout.Toggle("Unchanging Mood", character.unchangingMood);
            
            if (!character.unchangingMood) EditorGUI.BeginDisabledGroup(true);
            character.mood = (MoraleLevel)EditorGUILayout.EnumPopup("Mood", character.mood);
            if (!character.unchangingMood) EditorGUI.EndDisabledGroup();
            
            if (!character.unchangingMood)
            {
                EditorGUI.indentLevel++;

                character.maxMorale = EditorGUILayout.IntSlider("Max Morale", character.maxMorale, 3, 10);
                character.moraleValue = EditorGUILayout.IntSlider("Morale", character.moraleValue, 0, character.maxMorale);

                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(onDeathProp, new GUIContent("On Death"));

        EditorGUILayout.Separator();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(character);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
