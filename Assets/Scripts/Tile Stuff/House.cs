using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class House : Building
{
    public CharacterClass character;

    public List<BuildingDefense> defenses;

    public Character occupant
        => characters[(int)character];

    private Character[] characters;

    void Awake()
        => ResetCharactersArray();

    public void ResetCharactersArray()
    {
        if (characters == null)
        {
            characters = 
                GameObject
                    .FindGameObjectsWithTag("CharacterObject")
                    .Select(obj => obj?.GetComponent<Character>())
                    .Where(c => c != null)
                    .OrderBy(c => c.characterClass)
                    .Distinct()
                    .ToArray();
            
            if (Enum.GetValues(typeof(CharacterClass)).Length != characters.Length)
            {
                Debug.LogError($"Expected {Enum.GetValues(typeof(CharacterClass)).Length} character objects, got {characters.Length}");
            }
        }
    }

    public int defenseLevel
    {
        get => defenses.Select(defenseItem => defenseItem.value).Sum();
        set
        {
            int remainingPoints = value;
            defenses = defenses.TakeWhile(defenseItem => {
                if (remainingPoints <= 0) return false;
                if (defenseItem.value > remainingPoints) 
                {
                    defenseItem.value = remainingPoints;
                    defenseItem.damaged = true; // set damaged flag
                }
                remainingPoints -= defenseItem.value;
                return true;
            }).ToList();
        }
    }

    public static House AddTo(GameObject gameObject, string name, CharacterClass character, RectInt region)
    {
        House house = gameObject.AddComponent<House>() as House;
        house.buildingName = name;
        house.character = character;
        house.region = region;
        return house;
    }

    public override void OnClick()
    {
        if (debugClicks)
        {
            print($"Clicked the {character}'s house! (occupant={occupant})");
        }

        // TODO: update ui to display occupant data and enter house
        print($"Fill in UI hook for {occupant} here.");
    }
}

#if UNITY_EDITOR
#pragma warning disable 0618
[CustomEditor(typeof(House))]
public class HouseEditor : Editor
{
    private House house => target as House;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var oldLevel = house.defenseLevel;
        var newLevel = EditorGUILayout.IntField("Defense Level", oldLevel);
        if (newLevel < 0)
        {
            Debug.LogWarning("Cannot decrease defense level past 0.");
            house.defenseLevel = 0;
        }
        else if (oldLevel > newLevel)
        {
            house.defenseLevel = newLevel;
        }
        else if (oldLevel < newLevel)
        {
            Debug.LogWarning("Cannot increase defense level directly. \nAdd a defense object to the defenses list instead.");
        }

        using (new MaybeDisabledGroup())
        {
            if (!house.occupant)
            {
                house.ResetCharactersArray();
            }
            EditorGUILayout.ObjectField("Occupant", house.occupant, typeof(Character));
        }
    }
}
#endif