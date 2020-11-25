using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class House : Building
{
    public CharacterClass character;

    public List<BuildingDefense> defenses;
    public int defenseLevel
    {
        get => defenses.Select(defenseItem => defenseItem.value).Sum();
        set => defenses = 
            defenses.Aggregate(
                (list: new List<BuildingDefense>(), remaining: value), 
                (tuple, defenseItem) => {
                    if (tuple.remaining <= 0) {
                        // if there are no remaining points,
                        // don't add any more defenses
                        return tuple;
                    }

                    // add the next item to the new list
                    tuple.list.Add(defenseItem);

                    if (defenseItem.value > tuple.remaining) 
                    {
                        // if there isn't enough remaining, 
                        // decrease the value of the last item
                        defenseItem.value = tuple.remaining;
                        defenseItem.damaged = true; // set damaged flag
                    }
                    
                    // subtract the value of the defenseItem
                    tuple.remaining -= defenseItem.value;
                    
                    return tuple;
                })
                .list;
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