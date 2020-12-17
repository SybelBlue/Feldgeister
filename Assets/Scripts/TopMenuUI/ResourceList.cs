using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceList : MonoBehaviour
{
    public Character miner;
    public int salt = 2;
    public TMP_Text minerText;
    public int weapons = 0;
    public TMP_Text blacksmithText;
    public int enchantment = 0;
    public TMP_Text witchText;
    public int rations = 2;
    public Character farmer;
    public TMP_Text farmerText;
    public void ToggleDisplay()
    {
        //taken from https://answers.unity.com/questions/800940/toggle-gameobject-onoff-with-button.html
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void AddSalt(int moreSalt)
    {
        salt += moreSalt;
        minerText.text = "Salt: " + salt;
    }
    public void AddWeapons(int moreWeapons)
    {
        weapons += moreWeapons;
        blacksmithText.text = "Weapons: " + weapons;
    }
    public void AddEnchantment()
    {
        witchText.text = "Enchantments: " + (++enchantment);
    }
    public void AddRations(int moreRations)
    {
        rations += moreRations;
        farmerText.text = "Rations: " + rations;
    }

    public void ResetAll()
    {
        AddSalt(-salt);
        AddWeapons(-weapons);
        AddRations((-rations + 2));
        enchantment = 0;
        witchText.text = "Enchantments: " + enchantment;
        
        //default resource values
        AddSalt(2);
        if(miner.moraleValue > 1)
        {
            AddSalt(1);
        }
        if(farmer.moraleValue > 0)
        {
            AddRations(1);
        }
    }

}
