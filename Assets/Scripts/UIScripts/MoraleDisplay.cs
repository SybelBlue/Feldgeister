using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoraleDisplay : MonoBehaviour
{
    public TMP_Text morale;
    public AudioSource moraleUp;
    public AudioSource moraleDown;
    
    public void IncreaseMorale()
    {
        //from https://gamedevbeginner.com/how-to-play-audio-in-unity-with-examples/#play, 
        //https://answers.unity.com/questions/1327656/how-to-change-the-color-of-the-textmeshpro-gui-com.html,
        //https://answers.unity.com/questions/796881/c-how-can-i-let-something-happen-after-a-small-del.html
        morale.text = "+ morale";
        morale.color = new Color(50,150,50,255); //green
        moraleUp.Play();
        //yield return new WaitForSeconds(2);
        
    }
    public void DecreaseMorale() 
    {
        morale.text = "- morale";
        morale.color = new Color(150,50,50,255); //red
        moraleDown.PlayOneShot()
    }
}
