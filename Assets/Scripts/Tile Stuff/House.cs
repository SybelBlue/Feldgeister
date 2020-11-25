using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class House : Building
{
    public CharacterClass character;

    public List<BuildingDefense> defenses;

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
        print($"Clicked the {character}'s house!");
    }
}

#if UNITY_EDITOR
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
    }
}
#endif