using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class House : Building
{
    public CharacterClass character;

    public List<BuildingDefense> defenses;
    public int defenseLevel
    {
        get => defenses.Aggregate(0, (defenseLevel, defenseItem) => defenseLevel + defenseItem.value);
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
                    
                    if (defenseItem.value <= tuple.remaining) {
                        // if there's enough points, just subtract from remaining
                        // could become 0 if defenseItem.Value == tuple.remaining
                        tuple.remaining -= defenseItem.value;
                    } else {
                        // if there isn't, decrease the value of the last item
                        defenseItem.value = tuple.remaining;
                        // and zero out the remaining
                        tuple.remaining = 0;
                    }
                    
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