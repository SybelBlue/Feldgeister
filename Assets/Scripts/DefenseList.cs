using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseList : MonoBehaviour
{
    public TMP_Text blacksmith;
    public Character blacksmithChar;
    public TMP_Text witch;
    public Character witchChar;
    public TMP_Text miner;
    public Character minerChar;
    public TMP_Text farmer;
    public Character farmerChar;

    public TMP_Text watcher;
    public Character watcherChar;
    public void toggleDisplay()
    {
        DefenseList.setActive(!activeSelf);
    }
    public void UpdateAll(){
        blacksmith.text = "Blacksmith: " + blacksmithChar.house.defenseLevel;
        witch.text = "Witch: " + witchChar.house.defenseLevel;
        miner.text = "Miner: " + minerChar.house.defenseLevel;
        farmer.text = "Farmer: " + farmerChar.house.defenseLevel;
        watcher.text = "Watcher: " + watcherChar.house.defenseLevel;
    }
}
