using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseOccupant : MonoBehaviour 
{
    public TMP_Text nameText;

    public void UpdateDisplay(Character character){
        if(character) {
            this.gameObject.SetActive(true);
            nameText.text = character.name;
        }
        else {
            this.gameObject.SetActive(false);
        }
    }
}