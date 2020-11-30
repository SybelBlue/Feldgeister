using System;

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