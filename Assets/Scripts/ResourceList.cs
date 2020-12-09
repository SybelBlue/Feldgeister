// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class ResourceList : MonoBehaviour
// {
//     public int salt = 0;
//     public TMP_Text minerText;
//     public int weapons = 0;
//     public TMP_Text blacksmithText;
//     public int enchantment = 0;
//     public TMP_Text witchText;
//     public int rations = 2;
//     public TMP_Text farmerText;
//     public void ToggleDisplay()
//     {
//         this.setActive(!activeSelf);
//     }
//     public void AddSalt(int moreSalt)
//     {
//         salt += moreSalt;
//         minerText.text = "Salt: " + salt;
//     }
//     public void AddWeapons(int moreWeapons)
//     {
//         weapons += moreWeapons;
//         blacksmithText.text = "Weapons: " + weapons;
//     }
//     public void AddEnchantment()
//     {
//         witchText.text = "Enchantments: " + (++enchantment);
//     }
//     public void AddRations(int moreRations)
//     {
//         rations += moreRations;
//         farmerText.text = "Rations: " + rations;
//     }

//     public void ResetAll()
//     {
//         AddSalt(-salt);
//         AddWeapons(-weapons);
//         AddEnchantment(-enchantment);
//         AddRations((-rations + 2));
//     }

// }
