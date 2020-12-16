using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HungerList : MonoBehaviour
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
        blacksmith.text = "Blacksmith: " + blacksmithChar.hunger.ToString();
        witch.text = "Witch: " + witchChar.hunger.ToString();
        miner.text = "Miner: " + minerChar.hunger.ToString();
        farmer.text = "Farmer: " + farmerChar.hunger.ToString();
        watcher.text = "Watcher: " + watcherChar.hunger.ToString();
    }
}
