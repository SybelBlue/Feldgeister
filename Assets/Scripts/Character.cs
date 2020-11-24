using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary> An enum to represent all the character classes </summary>
[Serializable]
public enum CharacterClass
{
    Miner,
    Blacksmith,
    Witch,
    Watcher,
    Farmer,
    Mayor,
}

/// <summary> An enum to represent character hunger levels </summary>
[Serializable]
public enum HungerLevel
{
    Starving,
    Hungry,
    Sated,
    Full,
}

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

    public IMorale moraleMode;

    public CharacterEvent m_onDeath;

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
    [Serializable]
    private enum EditorMoraleMode
    {
        Standard,
        Unflinching,
        Terrorized
    }

    private SerializedProperty m_onDeathProp;

    private Character character {
        get => target as Character;
    }

    private EditorMoraleMode moraleMode {
        get {
            switch (character.moraleMode)
            {
                case UnflinchingMorale _:
                    return EditorMoraleMode.Unflinching;
                case TerrorizedMorale _:
                    return EditorMoraleMode.Terrorized;
                default:
                    return EditorMoraleMode.Standard;
            }
        }

        set {
            if (moraleMode == value) return;
            switch (value)
            {
                case EditorMoraleMode.Unflinching:
                    character.moraleMode = new UnflinchingMorale();
                    break;
                case EditorMoraleMode.Terrorized:
                    character.moraleMode = new TerrorizedMorale();
                    break;
                case EditorMoraleMode.Standard:
                    character.moraleMode = new StandardMorale();
                    break;
            }
        }
    }

    void OnEnable()
    {
        m_onDeathProp = serializedObject.FindProperty("m_onDeath");
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

            var newMode = (EditorMoraleMode)EditorGUILayout.EnumPopup("Morale Mode", moraleMode);
            moraleMode = newMode;
            if (newMode == EditorMoraleMode.Standard)
            {
                EditorGUI.indentLevel++;

                StandardMorale morale = character.moraleMode as StandardMorale ?? new StandardMorale();
                morale.maxMorale = (short)EditorGUILayout.IntField("Max Morale", morale.maxMorale);
                morale.morale = (short)EditorGUILayout.IntSlider("Morale", morale.morale, 0, morale.maxMorale);
                character.moraleMode = morale;

                EditorGUI.indentLevel++;
                
                if (character.moraleMode.unflinching) {
                    EditorGUILayout.LabelField("Unflinching");
                }

                if (character.moraleMode.terrorized) {
                    EditorGUILayout.LabelField("Terrorized");
                }

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Separator();

        if (character.characterClass != CharacterClass.Mayor)
        {
            EditorGUILayout.PropertyField(m_onDeathProp, new GUIContent("On Death"));
        }

        EditorGUILayout.Separator();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(character);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
