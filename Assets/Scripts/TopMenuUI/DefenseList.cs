using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public void ToggleDisplay()
    {
        this.gameObject.SetActive(!gameObject.activeInHierarchy);
        if(this.gameObject.activeInHierarchy)
        {
            UpdateAll();
        }
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void UpdateAll(){
        blacksmith.text = "Blacksmith: " + blacksmithChar.house.defenseLevel;
        witch.text = "Witch: " + witchChar.house.defenseLevel;
        miner.text = "Miner: " + minerChar.house.defenseLevel;
        farmer.text = "Farmer: " + farmerChar.house.defenseLevel;
        watcher.text = "Watcher: " + watcherChar.house.defenseLevel;
    }
}
