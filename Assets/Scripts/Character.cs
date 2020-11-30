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
    public MoraleLevel _mood;
    
    public MoraleLevel mood
    {
        get => unchangingMood ? 
                _mood :
                value * 4 >= maxMorale * 3 ?
                    MoraleLevel.Unflinching :
                    value * 4 <= maxMorale ?
                        MoraleLevel.Terrorized:
                        MoraleLevel.Normal;
        set => _mood = value;
    }

    [SerializeField]
    private int _value;
    
    public int value { 
        get => _value;
        set => _value = ClampMorale(value);
    }

    [SerializeField]
    private int _maxMorale = 6;
    public int maxMorale 
    { 
        get => _maxMorale;
        set {
            _maxMorale = Mathf.Max(0, value);
            this.value = _value;
        }
    }

        /// <summary> increases morale </summary>
    public void IncreaseMorale()
    {
        if (unchangingMood) return;
        value = ClampMorale(value + 1);
    }
    
    /// <summary> decreases morale </summary>
    public void DecreaseMorale()
    {
        if (unchangingMood) return;
        value = ClampMorale(value - 1);
    }
    
    /// <summary> plummets morale </summary>
    public void PlummetMorale()
    {
        if (unchangingMood) return;
        value = 0;
    }

    /// <summary> skyrockets morale </summary>
    public void SkyrocketMorale()
    {
        if (unchangingMood) return;
        value = maxMorale;
    }

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
                character.value = EditorGUILayout.IntSlider("Morale", character.value, 0, character.maxMorale);

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
