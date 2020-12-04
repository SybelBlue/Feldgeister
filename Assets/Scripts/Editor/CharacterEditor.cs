#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
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
        
        character.name = character.characterClass.ToString();

        character.characterClass = (CharacterClass)EditorGUILayout.EnumPopup("Class", character.characterClass);

        EditorGUILayout.Space();

        using (new DisabledGroup(character.immortal))
        {
            character.alive = EditorGUILayout.Toggle("Alive", character.immortal || character.alive);
        }

        if (character.alive)
        {
            EditorGUILayout.Space();

            // if farmer, can't change hunger level from max
            using (new DisabledGroup(character.characterClass == CharacterClass.Farmer))
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
        EditorGUILayout.PropertyField(onDeathProp, new GUIContent("On Death"));

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