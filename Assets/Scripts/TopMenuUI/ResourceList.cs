﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceList : MonoBehaviour
{
    public int salt = 0;
    public TMP_Text minerText;
    public int weapons = 0;
    public TMP_Text blacksmithText;
    public int enchantment = 0;
    public TMP_Text witchText;
    public int rations = 2;
    public TMP_Text farmerText;
    public void ToggleDisplay()
    {
        //taken from https://answers.unity.com/questions/800940/toggle-gameobject-onoff-with-button.html
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
    public void turnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void turnOff()
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
    }

}
