using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class House : Building
{

    public CharacterClass character;

    public List<BuildingDefense> defenses;

    public Character occupant
        => characters[(int)character];

    private Character[] characters;
    
    public CharacterEvent onHouseClick;

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
        house.character.house = house;
        house.region = region;

        return house;
    }

    public override void OnClick()
    {
        if (debugClicks)
        {
            print($"Clicked the {character}'s house! (occupant={occupant})");
        }

        onHouseClick.Invoke(occupant);
    }
}