using System;
using UnityEngine;

public class House : Building
{
    public CharacterClass character;

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