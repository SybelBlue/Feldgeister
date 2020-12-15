using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn;

public class MoraleDisplay : MonoBehaviour
{
    public TMP_Text morale;
    public AudioSource moraleUp;
    public AudioSource moraleDown;
    public float volume = 0.5f;
    
    public void IncreaseMoraleDisplay()
    {
        //from https://gamedevbeginner.com/how-to-play-audio-in-unity-with-examples/#play, 
        //https://answers.unity.com/questions/1327656/how-to-change-the-color-of-the-textmeshpro-gui-com.html,
        moraleUp.PlayOneShot(moraleUp.clip, volume);
        morale.color = new Color32(50,150,50,255); //green
        morale.text = "+ morale";
    }
    public void DecreaseMoraleDisplay() 
    {
        moraleDown.PlayOneShot(moraleDown.clip, volume);
        morale.color = new Color32(175,25,25,255); //red
        morale.text = "- morale";
    }

    void Update()
    {
        if(!moraleUp.isPlaying && !moraleDown.isPlaying)
        {
            morale.text = "";
        }
    }
}
